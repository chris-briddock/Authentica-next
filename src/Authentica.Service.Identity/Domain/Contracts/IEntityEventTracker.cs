namespace Authentica.Service.Identity.Domain.Contracts;

/// <summary>
/// Defines a contract for tracking domain events associated with an entity.
/// This interface provides a non-intrusive way to collect domain events
/// without requiring entities to inherit from a base class.
/// </summary>
public interface IEntityEventTracker
{
    /// <summary>
    /// Gets the unique identifier of the entity being tracked.
    /// </summary>
    string EntityId { get; }

    /// <summary>
    /// Gets the type of the entity being tracked.
    /// </summary>
    Type EntityType { get; }

    /// <summary>
    /// Adds a domain event to the tracker.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="domainEvent"/> is null.</exception>
    void AddEvent(IDomainEvent domainEvent);

    /// <summary>
    /// Gets all domain events associated with the entity.
    /// </summary>
    /// <returns>A read-only collection of domain events.</returns>
    IReadOnlyList<IDomainEvent> GetEvents();

    /// <summary>
    /// Clears all domain events from the tracker.
    /// </summary>
    /// <returns>The number of events that were cleared.</returns>
    int ClearEvents();

    /// <summary>
    /// Gets the count of pending domain events.
    /// </summary>
    int PendingEventCount { get; }

    /// <summary>
    /// Determines whether the tracker has any pending events.
    /// </summary>
    bool HasPendingEvents { get; }
}