using System;

namespace OneCalc.WebApi.Controllers.Users.V1.Responses
{
    public class UserResponse
    {
        public UserResponse(long userId, string email, string token, DateTime? expiresAt)
        {
            UserId = userId;
            Token = token;
            Email = email;
            ExpiresAtUtc = expiresAt;
        }
        public long UserId { get;}
        
        public string Email { get; }
        
        public string Token { get; }

        public DateTime? ExpiresAtUtc { get;  }
    }
}
