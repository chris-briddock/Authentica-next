using Authentica.Service.Identity.Domain.Contracts;
using Authentica.Service.Identity.Domain.Events;

namespace Authentica.Service.Identity.Domain.Extensions;

/// <summary>
/// Provides extension methods for managing domain events on entities
/// without requiring inheritance from a base class.
/// </summary>
public static class EntityEventExtensions
{
    private static readonly AsyncLocal<DomainEventsContext?> _currentContext = new();

    /// <summary>
    /// Gets or sets the current domain events context for the async operation.
    /// </summary>
    public static DomainEventsContext? CurrentContext
    {
        get => _currentContext.Value;
        set => _currentContext.Value = value;
    }

    /// <summary>
    /// Adds a domain event to the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to add the event to.</param>
    /// <param name="domainEvent">The domain event to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when entity or domainEvent is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no DomainEventsContext is available.</exception>
    public static void AddDomainEvent<TEntity>(this TEntity entity, IDomainEvent domainEvent) where TEntity : class
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (domainEvent == null)
            throw new ArgumentNullException(nameof(domainEvent));

        var context = CurrentContext ?? throw new InvalidOperationException(
            "No DomainEventsContext is available. Ensure DomainEventsContext is properly configured in your DI container or set CurrentContext.");

        var tracker = context.GetTrackerFor(entity);
        tracker.AddEvent(domainEvent);
    }

    /// <summary>
    /// Adds multiple domain events to the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to add events to.</param>
    /// <param name="domainEvents">The domain events to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when entity or domainEvents is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no DomainEventsContext is available.</exception>
    public static void AddDomainEvents<TEntity>(this TEntity entity, IEnumerable<IDomainEvent> domainEvents) where TEntity : class
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (domainEvents == null)
            throw new ArgumentNullException(nameof(domainEvents));

        var context = CurrentContext ?? throw new InvalidOperationException(
            "No DomainEventsContext is available. Ensure DomainEventsContext is properly configured in your DI container or set CurrentContext.");

        var tracker = context.GetTrackerFor(entity);
        foreach (var domainEvent in domainEvents)
        {
            if (domainEvent != null)
                tracker.AddEvent(domainEvent);
        }
    }

    /// <summary>
    /// Gets all domain events associated with the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to get events for.</param>
    /// <returns>A read-only list of domain events.</returns>
    /// <exception cref="ArgumentNullException">Thrown when entity is null.</exception>
    public static IReadOnlyList<IDomainEvent> GetDomainEvents<TEntity>(this TEntity entity) where TEntity : class
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var context = CurrentContext;
        if (context == null)
            return new List<IDomainEvent>();

        return context.GetEventsFor(entity);
    }

    /// <summary>
    /// Clears all domain events from the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to clear events from.</param>
    /// <returns>The number of events that were cleared.</returns>
    /// <exception cref="ArgumentNullException">Thrown when entity is null.</exception>
    public static int ClearDomainEvents<TEntity>(this TEntity entity) where TEntity : class
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var context = CurrentContext;
        if (context == null)
            return 0;

        return context.ClearEventsFor(entity);
    }

    /// <summary>
    /// Determines whether the entity has any pending domain events.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to check.</param>
    /// <returns>True if the entity has pending events; otherwise, false.</returns>
    public static bool HasDomainEvents<TEntity>(this TEntity entity) where TEntity : class
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var context = CurrentContext;
        if (context == null)
            return false;

        var events = context.GetEventsFor(entity);
        return events.Count > 0;
    }

    /// <summary>
    /// Creates a new DomainEventsContext scope for the current async operation.
    /// </summary>
    /// <returns>A disposable context that will be automatically cleaned up.</returns>
    public static DomainEventsContext CreateScope()
    {
        var context = new DomainEventsContext();
        CurrentContext = context;
        return context;
    }

    /// <summary>
    /// Executes the specified action within a DomainEventsContext scope.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    public static void WithContext(Action<DomainEventsContext> action)
    {
        using var context = CreateScope();
        action(context);
    }

    /// <summary>
    /// Executes the specified async function within a DomainEventsContext scope.
    /// </summary>
    /// <param name="func">The async function to execute.</param>
    public static async Task WithContextAsync(Func<DomainEventsContext, Task> func)
    {
        using var context = CreateScope();
        await func(context);
    }

    /// <summary>
    /// Executes the specified async function within a DomainEventsContext scope and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="func">The async function to execute.</param>
    /// <returns>The result of the async function.</returns>
    public static async Task<TResult> WithContextAsync<TResult>(Func<DomainEventsContext, Task<TResult>> func)
    {
        using var context = CreateScope();
        return await func(context);
    }
}