using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Savaglow.Models.Authentication
{
    public class Role : IdentityRole<string>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}