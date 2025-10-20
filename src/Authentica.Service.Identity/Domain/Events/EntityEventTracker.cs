using System.Collections.Concurrent;
using Authentica.Service.Identity.Domain.Contracts;

namespace Authentica.Service.Identity.Domain.Events;

/// <summary>
/// Default implementation of <see cref="IEntityEventTracker"/> that provides
/// thread-safe event tracking for entities.
/// </summary>
public sealed class EntityEventTracker : IEntityEventTracker
{
    private readonly ConcurrentQueue<IDomainEvent> _events = new();
    private readonly string _entityId;
    private readonly Type _entityType;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityEventTracker"/> class.
    /// </summary>
    /// <param name="entityId">The unique identifier of the entity.</param>
    /// <param name="entityType">The type of the entity.</param>
    /// <exception cref="ArgumentNullException">Thrown when entityId is null or empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when entityType is null.</exception>
    public EntityEventTracker(string entityId, Type entityType)
    {
        if (string.IsNullOrWhiteSpace(entityId))
            throw new ArgumentNullException(nameof(entityId));
        
        _entityId = entityId;
        _entityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
    }

    /// <inheritdoc />
    public string EntityId => _entityId;

    /// <inheritdoc />
    public Type EntityType => _entityType;

    /// <inheritdoc />
    public int PendingEventCount => _events.Count;

    /// <inheritdoc />
    public bool HasPendingEvents => !_events.IsEmpty;

    /// <inheritdoc />
    public void AddEvent(IDomainEvent domainEvent)
    {
        if (domainEvent == null)
            throw new ArgumentNullException(nameof(domainEvent));

        _events.Enqueue(domainEvent);
    }

    /// <inheritdoc />
    public IReadOnlyList<IDomainEvent> GetEvents()
    {
        return [.. _events];
    }

    /// <inheritdoc />
    public int ClearEvents()
    {
        var count = 0;
        while (_events.TryDequeue(out _))
        {
            count++;
        }
        return count;
    }

    /// <summary>
    /// Gets a string representation of the tracker for debugging purposes.
    /// </summary>
    /// <returns>A string containing entity information and event count.</returns>
    public override string ToString()
    {
        return $"EntityEventTracker for {_entityType.Name} ({_entityId}) with {PendingEventCount} pending events";
    }
}