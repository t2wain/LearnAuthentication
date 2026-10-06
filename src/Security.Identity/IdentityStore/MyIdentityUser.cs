using Microsoft.AspNetCore.Identity;
using Security.Identity.IdentityCore;

namespace Security.Identity.IdentityStore
{
    /// <summary>
    /// Documenting IdentityUser properties from library
    /// </summary>
    public class MyIdentityUser : IdentityUser, IMyUser
    {
        public override int AccessFailedCount { get => base.AccessFailedCount; set => base.AccessFailedCount = value; }
        public override string? ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }
        public override string? Email { get => base.Email; set => base.Email = value; }
        override public bool EmailConfirmed { get => base.EmailConfirmed; set => base.EmailConfirmed = value; }
        public override string Id { get => base.Id; set => base.Id = value; }
        override public bool LockoutEnabled { get => base.LockoutEnabled; set => base.LockoutEnabled = value; }
        public override DateTimeOffset? LockoutEnd { get => base.LockoutEnd; set => base.LockoutEnd = value; }
        override public string? NormalizedEmail { get => base.NormalizedEmail; set => base.NormalizedEmail = value; }
        override public string? NormalizedUserName { get => base.NormalizedUserName; set => base.NormalizedUserName = value; }
        override public string? PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }
        override public string? PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
        override public bool PhoneNumberConfirmed { get => base.PhoneNumberConfirmed; set => base.PhoneNumberConfirmed = value; }
        public override string? SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }
        public override bool TwoFactorEnabled { get => base.TwoFactorEnabled; set => base.TwoFactorEnabled = value; }
        public override string? UserName { get => base.UserName; set => base.UserName = value; }
    }
}
