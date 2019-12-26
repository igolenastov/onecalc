
using Microsoft.Extensions.DependencyInjection;
using OneCalc.Domain.Commands;
using OneCalc.Domain.Queries;
using OneCalc.Domain.Services;
using OneCalc.Domain.Validators;
using OneCalc.Infrastructure.Commands;
using OneCalc.Infrastructure.Queries;
using OneCalc.Infrastructure.Services;
using OneCalc.Infrastructure.Validators;

namespace OneCalc.Infrastructure.DependencyInjection
{
    public static class AddDependencyInjectionExtensions
    {
        public static IServiceCollection AddCalcServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<IHistoryQuery, HistoryQuery>();
            services.AddScoped<IUserQuery, UserQuery>();
            services.AddScoped<ICalculateService, CalculateService>();
        
            services.AddScoped<IHistoryCommand, HistoryCommand>();
           
            services.AddScoped<IOperationValidate, OperationValidate>();

            services.AddNodeServices();

            return services;
        }
    }
}
