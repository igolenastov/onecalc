using System;
using System.Collections.Generic;
using System.Text;

namespace OneCalc.Test.Integration.Model
{
    public class UserResponse : BaseResponse
    {
        public long UserId { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public DateTime? ExpiresAtUtc { get; set; }
    }
}
