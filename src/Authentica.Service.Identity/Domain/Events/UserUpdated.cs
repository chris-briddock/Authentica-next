using Authentica.Service.Identity.Domain.Contracts;

namespace Authentica.Service.Identity.Domain.Events;

/// <summary>
/// Represents a domain event that occurs when a user is updated.
/// </summary>
public sealed class UserUpdated : IDomainEvent
{
    /// <summary>
    /// Gets the unique identifier of the user that was updated.
    /// </summary>
    public string UserId { get; }

    /// <summary>
    /// Gets the properties that were changed during the update.
    /// </summary>
    public IReadOnlyDictionary<string, object?> ChangedProperties { get; }

    /// <summary>
    /// Gets the timestamp when the update occurred.
    /// </summary>
    public DateTime OccurredAt { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserUpdated"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="changedProperties">The properties that were changed.</param>
    /// <param name="occurredAt">The timestamp when the update occurred.</param>
    public UserUpdated(string userId, IDictionary<string, object?> changedProperties, DateTime occurredAt)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        ChangedProperties = new Dictionary<string, object?>(changedProperties);
        OccurredAt = occurredAt;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserUpdated"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="changedProperties">The properties that were changed.</param>
    public UserUpdated(string userId, IDictionary<string, object?> changedProperties)
        : this(userId, changedProperties, DateTime.UtcNow)
    {
    }
}