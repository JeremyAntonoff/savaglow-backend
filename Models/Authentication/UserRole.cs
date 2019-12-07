using Microsoft.AspNetCore.Identity;

namespace Savaglow.Models.Authentication
{
    public class UserRole : IdentityUserRole<string>
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}