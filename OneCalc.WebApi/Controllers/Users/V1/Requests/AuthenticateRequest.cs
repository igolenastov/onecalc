using System.ComponentModel.DataAnnotations;

namespace OneCalc.WebApi.Controllers.Users.V1.Requests
{
    /// <summary>
    /// Запрос на авторизацию пользователя
    /// </summary>
    public class AuthenticateRequest
    {
        /// <summary>
        /// Email пользователя
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
