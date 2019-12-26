using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OneCalc.Domain.Entities;
using OneCalc.Domain.Errors;
using OneCalc.Domain.Extensions;
using OneCalc.Domain.Queries;
using OneCalc.Domain.Security;
using OneCalc.Domain.Services;
using OneCalc.WebApi.Controllers.Users.V1.Requests;
using OneCalc.WebApi.Controllers.Users.V1.Responses;
using OneCalc.WebApi.Exceptions;
using OneCalc.WebApi.Extensions;

namespace OneCalc.WebApi.Controllers.Users.V1
{
    /// <summary>
    /// Взаимодействие с данными пользователя
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1"), ApiController]
    [Route("api/v{version:apiversion}/users")]
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtServices;
        private readonly IUserQuery _userQuery;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="jwtServices"></param>
        /// <param name="userQuery"></param>
        public UsersController(UserManager<ApplicationUser> userManager, IJwtService jwtServices, IUserQuery userQuery)
        {
            _userManager = userManager;
            _jwtServices = jwtServices;
            _userQuery = userQuery;
        }


        /// <summary>
        /// Получение токена зарегистрированного пользователя
        /// </summary>
        /// <param name="model">JSON строка с параметрами</param>
        /// <returns></returns>
        [HttpPost("token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserResponse), 200)]
        public async Task<IActionResult> Token([FromBody]AuthenticateRequest model)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException(ModelState);

            var userId = User.GetUserId();

            if (userId != null)
                throw new BadRequestException("you auth", ErrorCodes.Core.AuthError);

            var userWithEmail = await _userManager.FindByEmailAsync(model.Email);

            if (userWithEmail == null)
                throw new BadRequestException("user not found", ErrorCodes.Core.AuthError);

            var isAdmin = (await _userManager.GetRolesAsync(userWithEmail)).Contains(AuthorizationConstant.AuthorizedAdmin);

            var (token, expiresAt) = _jwtServices.GetJwtToken(userWithEmail, isAdmin);

            var response = new UserResponse(userWithEmail.Id, userWithEmail.Email, token, expiresAt);

            return Ok(response);
        }


        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="model">JSON строка с параметрами</param>
        /// <returns></returns>
        [HttpPost("create")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserResponse), 200)]
        public async Task<IActionResult> Create([FromBody]CreateRequest model)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException(ModelState);

            var userId = User.GetUserId();

            if (userId != null)
                throw new BadRequestException("you auth", ErrorCodes.Core.AuthError);


            var userWithEmail = await _userManager.FindByEmailAsync(model.Email);

            if (userWithEmail != null)
                throw new BadRequestException("user exists", ErrorCodes.Core.AuthError);


            var operations = model.Operations.ConvertToOperations();
            
            if (operations == null)
                throw new BadRequestException("not allowed operations", ErrorCodes.Core.AuthError);


            var result = await _userManager.CreateAsync(new ApplicationUser { Email = model.Email, UserName = model.Email, AllowOperation = operations.Value, CreatedAt = DateTime.UtcNow, EmailConfirmed = true}, model.Password);

            if (result.Succeeded)
            {
                userWithEmail = await _userManager.FindByEmailAsync(model.Email);

                var (token, expiresAt) = _jwtServices.GetJwtToken(userWithEmail);
                
                var response = new UserResponse(userWithEmail.Id, userWithEmail.Email, token, expiresAt);

                return Ok(response);
            }

            throw new BadRequestException("error create user", ErrorCodes.Core.AuthError);
        }
     

        /// <summary>
        /// Получение информации о пользователе
        /// </summary>
        /// <returns></returns>
        [HttpGet("info")]
        [Authorize(AuthorizationConstant.AuthorizedUser)]
        [ProducesResponseType(typeof(UserResponse), 200)]
        public async Task<IActionResult> Info()
        {
            var user = await _userManager.GetUserAsync(User);

            var response = new UserResponse(user.Id, user.Email, null, null);

            return Ok(response);
        }


        /// <summary>
        /// Возвращает всех пользователей (только для админа)
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize(AuthorizationConstant.AuthorizedAdmin)]
        [ProducesResponseType(typeof(List<UserResponse>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userQuery.GetAllAsync();

            var result = new List<UserResponse>(users.Count);

            foreach (var user in users)
            {
                var (token, expiresAt) = _jwtServices.GetJwtToken(user);

                result.Add(new UserResponse(user.Id, user.Email, token, expiresAt));
            }

            return Ok(result);
        }
    }
}
