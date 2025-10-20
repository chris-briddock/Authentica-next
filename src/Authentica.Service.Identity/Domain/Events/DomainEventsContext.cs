using System.Collections.Concurrent;
using Authentica.Service.Identity.Domain.Contracts;

namespace Authentica.Service.Identity.Domain.Events;

/// <summary>
/// Provides a scoped context for managing domain events across all entities
/// within a single operation or request scope.
/// </summary>
public sealed class DomainEventsContext : IDisposable
{
    private readonly ConcurrentDictionary<string, IEntityEventTracker> _trackers = new();
    private bool _disposed;

    /// <summary>
    /// Gets or creates an event tracker for the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to track events for.</param>
    /// <returns>An event tracker for the entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown when entity is null.</exception>
    public IEntityEventTracker GetTrackerFor<TEntity>(TEntity entity) where TEntity : class
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var entityId = GetEntityId(entity);
        var entityType = typeof(TEntity);
        var key = $"{entityType.FullName}:{entityId}";

        return _trackers.GetOrAdd(key, _ => new EntityEventTracker(entityId, entityType));
    }

    /// <summary>
    /// Gets all domain events from all tracked entities.
    /// </summary>
    /// <returns>A read-only list of all domain events.</returns>
    public IReadOnlyList<IDomainEvent> GetAllEvents()
    {
        return [.. _trackers.Values.SelectMany(tracker => tracker.GetEvents())];
    }

    /// <summary>
    /// Gets all domain events for a specific entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to get events for.</typeparam>
    /// <returns>A read-only list of domain events for the specified entity type.</returns>
    public IReadOnlyList<IDomainEvent> GetEventsFor<TEntity>() where TEntity : class
    {
        var entityType = typeof(TEntity);
        return [.. _trackers.Values
            .Where(tracker => tracker.EntityType == entityType)
            .SelectMany(tracker => tracker.GetEvents())];
    }

    /// <summary>
    /// Gets all domain events for a specific entity instance.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to get events for.</param>
    /// <returns>A read-only list of domain events for the specified entity.</returns>
    public IReadOnlyList<IDomainEvent> GetEventsFor<TEntity>(TEntity entity) where TEntity : class
    {
        if (entity == null)
            return [];

        var entityId = GetEntityId(entity);
        var entityType = typeof(TEntity);
        var key = $"{entityType.FullName}:{entityId}";

        return _trackers.TryGetValue(key, out var tracker) 
            ? tracker.GetEvents() 
            : [];
    }

    /// <summary>
    /// Clears all domain events from all tracked entities.
    /// </summary>
    /// <returns>The total number of events that were cleared.</returns>
    public int ClearAllEvents()
    {
        var totalCleared = 0;
        
        foreach (var tracker in _trackers.Values)
        {
            totalCleared += tracker.ClearEvents();
        }
        return totalCleared;
    }

    /// <summary>
    /// Clears all domain events for a specific entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to clear events for.</typeparam>
    /// <returns>The number of events that were cleared.</returns>
    public int ClearEventsFor<TEntity>() where TEntity : class
    {
        var entityType = typeof(TEntity);
        var trackersToClear = _trackers.Values
            .Where(tracker => tracker.EntityType == entityType)
            .ToList();

        return trackersToClear.Sum(tracker => tracker.ClearEvents());
    }

    /// <summary>
    /// Clears all domain events for a specific entity instance.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to clear events for.</param>
    /// <returns>The number of events that were cleared.</returns>
    public int ClearEventsFor<TEntity>(TEntity entity) where TEntity : class
    {
        if (entity == null)
            return 0;

        var entityId = GetEntityId(entity);
        var entityType = typeof(TEntity);
        var key = $"{entityType.FullName}:{entityId}";

        return _trackers.TryGetValue(key, out var tracker) 
            ? tracker.ClearEvents() 
            : 0;
    }

    /// <summary>
    /// Gets the total count of pending events across all tracked entities.
    /// </summary>
    public int TotalPendingEventCount => _trackers.Values.Sum(tracker => tracker.PendingEventCount);

    /// <summary>
    /// Determines whether any entities have pending events.
    /// </summary>
    public bool HasPendingEvents => _trackers.Values.Any(tracker => tracker.HasPendingEvents);

    /// <summary>
    /// Gets all entities that have pending events.
    /// </summary>
    public IReadOnlyList<(Type EntityType, string EntityId, int EventCount)> GetEntitiesWithPendingEvents()
    {
        return _trackers.Values
            .Where(tracker => tracker.HasPendingEvents)
            .Select(tracker => (tracker.EntityType, tracker.EntityId, tracker.PendingEventCount))
            .ToList();
    }

    private static string GetEntityId<TEntity>(TEntity entity) where TEntity : class
    {
        // Handle common entity patterns
        var entityType = typeof(TEntity);
        
        // Try to get Id property
        var idProperty = entityType.GetProperty("Id") ?? 
                        entityType.GetProperty("ID") ??
                        entityType.GetProperty($"{entityType.Name}Id") ??
                        entityType.GetProperty($"{entityType.Name}ID");

        if (idProperty != null && idProperty.CanRead)
        {
            var idValue = idProperty.GetValue(entity);
            return idValue?.ToString() ?? Guid.NewGuid().ToString();
        }

        // Fallback to hash code for entities without Id property
        return entity.GetHashCode().ToString();
    }

    /// <summary>
    /// Disposes the context and clears all tracked events.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        ClearAllEvents();
        _trackers.Clear();
        _disposed = true;
    }
}