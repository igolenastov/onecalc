using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using OneCalc.Domain.Errors;
using OneCalc.WebApi.Exceptions;

namespace OneCalc.WebApi.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        /// <summary>
        /// При ошибках сервера всегда отдаем 200 ответ
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        var badRequest = contextFeature.Error as BadRequestException;

                        IErrorCode errorCode = badRequest?.Error ?? ErrorCodes.Core.Unrecognized;

                        var result = new 
                        {
                            errorCode?.ErrorCode,
                            errorCode?.ErrorTitle,
                            ErrorMessage = contextFeature.Error.ToString(),
                        };

                        var sResult = JsonSerializer.Serialize(result);

                        await context.Response.WriteAsync(sResult);
                    }
                });
            });
        }
    }
}
