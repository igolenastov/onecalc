using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using OneCalc.Domain.Enums;

namespace OneCalc.Domain.Entities
{
    public class History 
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string Input { get; set; }
        public string Result { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
