using System.ComponentModel.DataAnnotations;

namespace OneCalc.WebApi.Controllers.Users.V1.Requests
{
    /// <summary>
    /// Запрос на создание пользователя
    /// </summary>
    public class CreateRequest : AuthenticateRequest
    {
        /// <summary>
        /// Поддерживаемые операция для пользователя (обязательно хотя бы одна операция), формат - +,-,*,/,(,)
        /// </summary>
        [Required]
        public string Operations { get; set; }
    }
}
