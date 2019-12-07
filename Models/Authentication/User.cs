using System;
using System.Collections.Generic;
using Savaglow.Models.Authentication;
using Savaglow.Models.Ledger;
using Microsoft.AspNetCore.Identity;

namespace Savaglow.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<LedgerItem> LedgerItems { get; set; }
    }
}