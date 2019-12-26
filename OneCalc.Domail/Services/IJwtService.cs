using System;
using OneCalc.Domain.Entities;

namespace OneCalc.Domain.Services
{
    public interface IJwtService
    {
        (string, DateTime) GetJwtToken(ApplicationUser user, bool isAdmin = false);
    }
}
