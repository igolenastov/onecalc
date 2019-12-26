using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Logging;
using OneCalc.Domain.AppSettings;
using OneCalc.Domain.Entities;
using OneCalc.Domain.Security;
using OneCalc.Domain.Services;

namespace OneCalc.Infrastructure.Services
{
    public class JwtService: IJwtService
    {
        private readonly JwtSetting _appSettings;

        public JwtService(IOptionsSnapshot<JwtSetting> appSettings)
        {
            _appSettings = appSettings?.Value;
        }

        public (string, DateTime) GetJwtToken(ApplicationUser user, bool isAdmin = false)
        {
            if (!IdentityModelEventSource.ShowPII)
                IdentityModelEventSource.ShowPII = true;

            var expiresAt = DateTime.UtcNow.AddDays(7);

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(AuthorizationConstant.ApplicationClaimTypes.Operation , user.AllowOperation.ToString())
            };

            if (isAdmin)
                claims.Add(new Claim(AuthorizationConstant.ApplicationClaimTypes.Admin, AuthorizationConstant.ApplicationClaimTypes.True));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var  token = tokenHandler.CreateToken(tokenDescriptor);
            
            return (tokenHandler.WriteToken(token), expiresAt);
        }
    }
}
