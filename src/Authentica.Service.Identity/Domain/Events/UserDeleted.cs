using Authentica.Service.Identity.Domain.Contracts;

namespace Authentica.Service.Identity.Domain.Events;

/// <summary>
/// Represents a domain event that occurs when a user is deleted.
/// </summary>
public sealed class UserDeleted : IDomainEvent
{
    /// <summary>
    /// Gets the unique identifier of the user that was deleted.
    /// </summary>
    public string UserId { get; }

    /// <summary>
    /// Gets the username of the deleted user.
    /// </summary>
    public string UserName { get; }

    /// <summary>
    /// Gets the email of the deleted user.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Gets the timestamp when the deletion occurred.
    /// </summary>
    public DateTime OccurredAt { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDeleted"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="userName">The username of the deleted user.</param>
    /// <param name="email">The email of the deleted user.</param>
    /// <param name="occurredAt">The timestamp when the deletion occurred.</param>
    public UserDeleted(string userId, string userName, string email, DateTime occurredAt)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        OccurredAt = occurredAt;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDeleted"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="userName">The username of the deleted user.</param>
    /// <param name="email">The email of the deleted user.</param>
    public UserDeleted(string userId, string userName, string email)
        : this(userId, userName, email, DateTime.UtcNow)
    {
    }
}