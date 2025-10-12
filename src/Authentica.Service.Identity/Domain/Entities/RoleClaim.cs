using Authentica.Service.Identity.Domain.Contracts;
using Authentica.Service.Identity.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Authentica.Service.Identity.Domain.Entities;

/// <inheritdoc/>
public sealed class RoleClaim : IdentityRoleClaim<string>,
                                IEntityCreationStatus<string>,
                                IEntityModificationStatus<string>,
                                IEntityDeletionStatus<string>


{
    
    public new string Id { get; set; } = Guid.NewGuid().ToString();
    public override string RoleId { get; set; } = default!;
    public override string? ClaimType { get; set; }
    public override string? ClaimValue { get; set; }
    public string ConcurrencyStamp { get; set; } = default!;
    public EntityCreationStatus<string> EntityCreationStatus { get; set; } = default!;
    public EntityModificationStatus<string> EntityModificationStatus { get; set; } = default!;
    public EntityDeletionStatus<string> EntityDeletionStatus { get; set; } = default!;

    public Role Role { get; set; } = default!;
}
