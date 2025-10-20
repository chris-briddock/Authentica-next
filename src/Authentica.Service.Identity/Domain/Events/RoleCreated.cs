using Authentica.Service.Identity.Domain.Contracts;

namespace Authentica.Service.Identity.Domain.Events;

/// <summary>
/// Represents a domain event that occurs when a role is created.
/// </summary>
public sealed class RoleCreated : IDomainEvent
{
    /// <summary>
    /// Gets the unique identifier of the role that was created.
    /// </summary>
    public string RoleId { get; }

    /// <summary>
    /// Gets the name of the created role.
    /// </summary>
    public string RoleName { get; }

    /// <summary>
    /// Gets the timestamp when the role was created.
    /// </summary>
    public DateTime OccurredAt { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleCreated"/> class.
    /// </summary>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <param name="roleName">The name of the created role.</param>
    /// <param name="occurredAt">The timestamp when the role was created.</param>
    public RoleCreated(string roleId, string roleName, DateTime occurredAt)
    {
        RoleId = roleId ?? throw new ArgumentNullException(nameof(roleId));
        RoleName = roleName ?? throw new ArgumentNullException(nameof(roleName));
        OccurredAt = occurredAt;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleCreated"/> class.
    /// </summary>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <param name="roleName">The name of the created role.</param>
    public RoleCreated(string roleId, string roleName)
        : this(roleId, roleName, DateTime.UtcNow)
    {
    }
}