using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using OneCalc.Domain.Security;

namespace OneCalc.WebApi.Security
{
    public static class AuthorizationPolicies
    {
        public static void Configure(AuthorizationOptions options)
        {
            options.AddPolicy(AuthorizationConstant.AuthorizedUser, policy =>
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser());

            options.AddPolicy(AuthorizationConstant.AuthorizedAdmin, policy =>
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .RequireClaim(AuthorizationConstant.ApplicationClaimTypes.Admin));
        }
    }
}
