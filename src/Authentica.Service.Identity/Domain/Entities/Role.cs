using Authentica.Service.Identity.Domain.Contracts;
using Authentica.Service.Identity.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Authentica.Service.Identity.Domain.Entities;

/// <summary>
/// Represents an application role, including identity and audit metadata for creation, modification, and deletion.
/// </summary>
public sealed class Role : IdentityRole<string>,
                           IEntityCreationStatus<string>,
                           IEntityModificationStatus<string>,
                           IEntityDeletionStatus<string>
{
    /// <summary>
    /// Unique identifier assigned to the role.
    /// </summary>
    public override string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Display name associated with the role.
    /// </summary>
    public override string? Name { get; set; } = default!;

    /// <summary>
    /// Normalized name used for role lookups and comparisons.
    /// </summary>
    public override string? NormalizedName { get; set; } = default!;

    /// <summary>
    /// Value used for concurrency checks to ensure data integrity.
    /// </summary>
    public override string? ConcurrencyStamp { get; set; } = default!;

    /// <summary>
    /// Metadata describing when and by whom the role was created.
    /// </summary>
    public EntityCreationStatus<string> EntityCreationStatus { get; set; } = default!;

    /// <summary>
    /// Metadata describing the most recent modification, including timestamp and user.
    /// </summary>
    public EntityModificationStatus<string> EntityModificationStatus { get; set; } = default!;

    /// <summary>
    /// Metadata describing soft deletion status, including deletion flag, timestamp, and user.
    /// </summary>
    public EntityDeletionStatus<string> EntityDeletionStatus { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyCollection<UserRole> UserRoles { get; set; } = [];

    public IReadOnlyCollection<RoleClaim> RoleClaims { get; set; } = [];
}