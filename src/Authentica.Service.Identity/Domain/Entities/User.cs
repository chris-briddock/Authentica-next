using Authentica.Service.Identity.Domain.Contracts;
using Authentica.Service.Identity.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Authentica.Service.Identity.Domain.Entities;

public sealed class User : IdentityUser<string>, 
                           IEntityCreationStatus<string>,
                           IEntityDeletionStatus<string>,
                           IEntityModificationStatus<string>
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();
    public override string? UserName { get; set; }
    public override string? NormalizedUserName { get; set; }
    public override string? Email { get; set; }
    public override string? NormalizedEmail { get; set; }
    public override bool EmailConfirmed { get; set; }
    public override string? PasswordHash { get; set; }
    public override string? SecurityStamp { get; set; }
    public override string? PhoneNumber { get; set; }
    public override bool PhoneNumberConfirmed { get; set; }
    public override bool TwoFactorEnabled { get; set; }
    public override bool LockoutEnabled { get; set; }
    public override DateTimeOffset? LockoutEnd { get; set; }
    public override string? ConcurrencyStamp { get; set; } = default!;

    public Address Address { get; set; } = default!;
    public EntityCreationStatus<string> EntityCreationStatus { get; set; } = default!;
    public EntityDeletionStatus<string> EntityDeletionStatus { get; set; } = default!;
    public EntityModificationStatus<string> EntityModificationStatus { get; set; } = default!;
    public IReadOnlyCollection<UserRole> UserRoles { get; set; } = [];
    public IReadOnlyCollection<UserClaim> UserClaims { get; set; } = [];
    public IReadOnlyCollection<UserLogin> UserLogins { get; set; } = [];
    public IReadOnlyCollection<UserToken> UserTokens { get; set; } = [];

}
