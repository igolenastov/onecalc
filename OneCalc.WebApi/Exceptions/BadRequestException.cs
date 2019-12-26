using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OneCalc.Domain.Errors;

namespace OneCalc.WebApi.Exceptions
{
    /// <summary>
    /// Для формирования сообщения при ошибках на сайте
    /// </summary>
    public class BadRequestException: Exception
    {
        public IErrorCode Error { get; set; }

        public BadRequestException(string message, IErrorCode errorCode = null) : base(message)
        {
            Error = errorCode ?? ErrorCodes.Core.ValidationError;
        }

        public BadRequestException(IEnumerable<KeyValuePair<string, ModelStateEntry>> modelState)
        {
            Error = ErrorCodes.Core.ValidationError;

           var errors =  modelState.Where(x => x.Value.ValidationState == ModelValidationState.Invalid)
                .Select(x => new 
                {
                    Name = string.IsNullOrEmpty(x.Key) ? "model" : x.Key,
                    Message = string.Join(", ",
                        x.Value.Errors.Select(e =>
                            string.IsNullOrEmpty(e.ErrorMessage)
                                ? e.Exception?.Message
                                : e.ErrorMessage))
                }).ToList();


           ErrorMessage = JsonSerializer.Serialize(errors);
        }

        private string ErrorMessage { get; set; }
      
        public override string ToString()
        {
            return ErrorMessage ?? Message;
        }
    }
}
