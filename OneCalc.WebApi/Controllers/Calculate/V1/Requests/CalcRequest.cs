using System.ComponentModel.DataAnnotations;

namespace OneCalc.WebApi.Controllers.Calculate.V1.Requests
{
    public class CalcRequest
    {
        /// <summary>
        /// Математическая операция
        /// </summary>
        [Required]
        public string Calculate { get; set; }

    }
}
