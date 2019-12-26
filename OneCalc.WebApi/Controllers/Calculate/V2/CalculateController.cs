using Microsoft.AspNetCore.Mvc;
using OneCalc.Domain.Enums;
using OneCalc.Domain.Errors;
using OneCalc.Domain.Services;
using OneCalc.Domain.Validators;
using OneCalc.WebApi.Controllers.Calculate.V1.Requests;
using OneCalc.WebApi.Exceptions;
using OneCalc.WebApi.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using OneCalc.Domain.Extensions;
using OneCalc.Domain.Queries;
using OneCalc.Domain.Security;
using OneCalc.WebApi.Controllers.Calculate.V1.Responses;

namespace OneCalc.WebApi.Controllers.Calculate.V2
{
    /// <summary>
    /// Калькулятор
    /// </summary>
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2"), ApiController]
    [Route("api/v{version:apiversion}/calculate")]
    [Produces("application/json")]
    [Authorize(AuthorizationConstant.AuthorizedUser)]
    public class CalculateV2Controller : Controller
    {
        private readonly IOperationValidate _operationValidate;
        private readonly ICalculateService _calculateService;
        private readonly IHistoryQuery _historyQuery;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationValidate"></param>
        /// <param name="calculateService"></param>
        /// <param name="historyQuery"></param>
        public CalculateV2Controller(IOperationValidate operationValidate, ICalculateService calculateService, IHistoryQuery historyQuery)
        {
            _operationValidate = operationValidate;
            _calculateService = calculateService;
            _historyQuery = historyQuery;
        }


        /// <summary>
        /// Калькулятор, поддерживает операции + - * /, а так же работу с круглыми скобкам
        /// </summary>
        /// <param name="model">JSON строка с параметрами</param>
        /// <returns></returns>
        [HttpPost("execute")]
        [ProducesResponseType(typeof(CalcResponse), 200)]
        public async Task<IActionResult> Execute([FromBody]CalcRequest model)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException(ModelState);

            var operations = User.GetOperations();

            if (operations == OperationEnum.Non)
                throw new BadRequestException("operations not allowed", ErrorCodes.Core.CalcError);

            var allowOperations = new List<OperationEnum> { OperationEnum.Div, OperationEnum.Mul, OperationEnum.Minus, OperationEnum.Plus, OperationEnum.ParanthesesLeft, OperationEnum.ParanthesesRight };

            // проверка на поддержку в методе операций
            var (isValid, symbol) = _operationValidate.IsValidAllow(model.Calculate, allowOperations.ToEnumFlags());

            if (!isValid)
                throw new BadRequestException($"'{symbol}' operations not allowed", ErrorCodes.Core.CalcError);

            foreach (var operation in allowOperations)
            {
                if (!_operationValidate.IsValid(operation, model.Calculate, operations))
                {
                    var symbolOperation = operation.GetType().GetMember(operation.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>()?.Name ?? string.Empty;

                    throw new BadRequestException($"'{symbolOperation}' operations not allowed", ErrorCodes.Core.CalcError);
                }
            }

            var userId = User.GetUserId();

            var result = await _calculateService.CalculateAsync(userId.Value, model.Calculate);

            return Ok(new CalcResponse(model.Calculate, result));
        }
       
    }
}
