using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using OneCalc.Domain.Enums;

namespace OneCalc.Domain.Entities
{
    public class ApplicationUser : IdentityUser<long>
    {
        public OperationEnum AllowOperation { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<IdentityUserRole<long>> Roles { get; set; }

        public virtual ICollection<History> Histories { get; set; }

    }
}
