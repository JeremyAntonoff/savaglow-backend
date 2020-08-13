using System;
using Savaglow.Models;
using Savaglow.Models.Authentication;
using Savaglow.Models.Ledger;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Savaglow.Data
{
    public class DataContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {

        public DataContext(DbContextOptions options) : base(options) { 
        }
        public DbSet<LedgerItem> LedgerItems { get; set; }
        public DbSet<RecurringLedgerItem> RecurringLedgerItems { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<LedgerItem>()
                .Property(e => e.TransactionType)
                .HasConversion(
                    v => v.ToString(),
                    v => (TransactionType)Enum.Parse(typeof(TransactionType), v));

            builder
                .Entity<RecurringLedgerItem>()
                .Property(e => e.TransactionType)
                .HasConversion(
                    v => v.ToString(),
                    v => (TransactionType)Enum.Parse(typeof(TransactionType), v));

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(uR => new { uR.UserId, uR.RoleId });
                userRole.HasOne(uR => uR.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(uR => uR.RoleId)
                .IsRequired();

                userRole.HasOne(uR => uR.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(uR => uR.UserId)
                .IsRequired();
            });


        }

    }
}