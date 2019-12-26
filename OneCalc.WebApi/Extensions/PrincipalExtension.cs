using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OneCalc.Domain.Enums;
using OneCalc.Domain.Security;

namespace OneCalc.WebApi.Extensions
{
    public static class PrincipalExtension
    {
        public static long? GetUserId(this ClaimsPrincipal principal)
        {
            var id = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (long.TryParse(id, out var lid))
                return lid;

            return null;
        }

        public static OperationEnum GetOperations(this ClaimsPrincipal principal)
        {
            var id = principal.Claims.FirstOrDefault(x => x.Type == AuthorizationConstant.ApplicationClaimTypes.Operation)?.Value;

            if (Enum.TryParse(id, out OperationEnum op))
                return op;

            return OperationEnum.Non;
        }
    }
}
