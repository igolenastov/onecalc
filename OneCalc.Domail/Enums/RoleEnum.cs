using System.ComponentModel.DataAnnotations;
using OneCalc.Domain.Security;

namespace OneCalc.Domain.Enums
{
    public enum RoleEnum
    {
        [Display(Name = "Администратор", Description = AuthorizationConstant.AuthorizedAdmin)]
        Administrator = 10,
    }
}
