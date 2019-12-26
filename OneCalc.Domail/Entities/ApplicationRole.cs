using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using OneCalc.Domain.Enums;

namespace OneCalc.Domain.Entities
{
    public class ApplicationRole : IdentityRole<long>
    {
        public RoleEnum Role { get; set; }

        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Navigation property for the users in this role.
        /// </summary>
        public virtual ICollection<IdentityUserRole<long>> Users { get; set; }
    }
}
