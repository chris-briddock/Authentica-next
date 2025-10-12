namespace Authentica.Service.Identity.Domain.Entities;

/// <summary>
/// Represents the link between a user and their session.
/// </summary>
public sealed class UserSession : UserSession<string>
{
    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    public User User { get; set; } = default!;
    /// <summary>
    /// Gets or sets the session.
    /// </summary>
    public Session Session { get; set; } = default!;
}

public abstract class UserSession<TKey>
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public virtual TKey Id { get; set; } = default!;

    public virtual TKey UserId { get; set; } = default!;

    public virtual TKey SessionId { get; set; } = default!;
}
