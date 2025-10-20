using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Authentica.Service.Identity.Domain.Extensions;
using Authentica.Service.Identity.Domain.Contracts;

namespace Authentica.Service.Identity.Persistence;

/// <summary>
/// Entity Framework Core interceptor that automatically collects and dispatches
/// domain events when entities are saved to the database.
/// </summary>
public sealed class DomainEventsSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IDomainEventDispatcher<IDomainEvent> _eventDispatcher;
    private readonly ILogger<DomainEventsSaveChangesInterceptor> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEventsSaveChangesInterceptor"/> class.
    /// </summary>
    /// <param name="eventDispatcher">The domain event dispatcher.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    public DomainEventsSaveChangesInterceptor(
        IDomainEventDispatcher<IDomainEvent> eventDispatcher,
        ILogger<DomainEventsSaveChangesInterceptor> logger)
    {
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Called before <see cref="DbContext.SaveChanges"/> or <see cref="DbContext.SaveChangesAsync"/>
    /// is called to collect domain events from entities.
    /// </summary>
    /// <param name="eventData">Contextual information about the <see cref="DbContext"/> being used.</param>
    /// <param name="result">The result of calling <see cref="DbContext.SaveChanges"/> or <see cref="DbContext.SaveChangesAsync"/>.</param>
    /// <returns>The interceptor result.</returns>
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
        {
            CollectDomainEvents(eventData.Context);
        }

        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    /// Called before <see cref="DbContext.SaveChangesAsync"/> is called to collect domain events from entities.
    /// </summary>
    /// <param name="eventData">Contextual information about the <see cref="DbContext"/> being used.</param>
    /// <param name="result">The result of calling <see cref="DbContext.SaveChangesAsync"/>.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The interceptor result.</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            CollectDomainEvents(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Called after <see cref="DbContext.SaveChanges"/> or <see cref="DbContext.SaveChangesAsync"/>
    /// has completed successfully to dispatch the collected domain events.
    /// </summary>
    /// <param name="eventData">Contextual information about the <see cref="DbContext"/> being used.</param>
    /// <param name="result">The result of calling <see cref="DbContext.SaveChanges"/> or <see cref="DbContext.SaveChangesAsync"/>.</param>
    /// <returns>The interception result.</returns>
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        if (eventData.Context is not null)
        {
            _ = DispatchDomainEventsAsync(eventData.Context).ConfigureAwait(false);
        }

        return base.SavedChanges(eventData, result);
    }

    /// <summary>
    /// Called after <see cref="DbContext.SaveChangesAsync"/> has completed successfully to dispatch the collected domain events.
    /// </summary>
    /// <param name="eventData">Contextual information about the <see cref="DbContext"/> being used.</param>
    /// <param name="result">The result of calling <see cref="DbContext.SaveChangesAsync"/>.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The interception result.</returns>
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            await DispatchDomainEventsAsync(eventData.Context, cancellationToken).ConfigureAwait(false);
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Called when <see cref="DbContext.SaveChanges"/> or <see cref="DbContext.SaveChangesAsync"/>
    /// has failed to clear any collected domain events.
    /// </summary>
    /// <param name="eventData">Contextual information about the <see cref="DbContext"/> being used.</param>
    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        if (eventData.Context is not null)
        {
            ClearDomainEvents(eventData.Context);
        }

        base.SaveChangesFailed(eventData);
    }

    /// <summary>
    /// Called when <see cref="DbContext.SaveChangesAsync"/> has failed to clear any collected domain events.
    /// </summary>
    /// <param name="eventData">Contextual information about the <see cref="DbContext"/> being used.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task SaveChangesFailedAsync(
        DbContextErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            await Task.Run(() => ClearDomainEvents(eventData.Context), cancellationToken).ConfigureAwait(false);
        }

        await base.SaveChangesFailedAsync(eventData, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Collects domain events from all tracked entities in the context.
    /// </summary>
    /// <param name="context">The database context.</param>
    private void CollectDomainEvents(DbContext context)
    {
        try
        {
            // Ensure we have a domain events context
            using var domainEventsScope = EntityEventExtensions.CreateScope();
            var domainEventsContext = EntityEventExtensions.CurrentContext!;

            // Collect events from all entity entries
            var entries = context.ChangeTracker.Entries()
                .Where(e => e.Entity is not null && (
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                if (entity == null) continue;

                // The entity should already have domain events added to it via the extension methods
                // We just need to ensure it's tracked in our domain events context
                var tracker = domainEventsContext.GetTrackerFor(entity);
                _logger.LogDebug(
                    "Entity {EntityType} with ID {EntityId} has {EventCount} pending events",
                    tracker.EntityType.Name,
                    tracker.EntityId,
                    tracker.PendingEventCount);
            }

            _logger.LogInformation(
                "Collected domain events from {EntityCount} entities. Total events: {EventCount}",
                entries.Count(),
                domainEventsContext.TotalPendingEventCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while collecting domain events");
            throw;
        }
    }

    /// <summary>
    /// Dispatches all collected domain events using the configured dispatcher.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task DispatchDomainEventsAsync(DbContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var domainEventsContext = EntityEventExtensions.CurrentContext;
            if (domainEventsContext == null || !domainEventsContext.HasPendingEvents)
            {
                _logger.LogDebug("No domain events to dispatch");
                return;
            }

            var allEvents = domainEventsContext.GetAllEvents();
            if (!allEvents.Any())
            {
                _logger.LogDebug("No domain events found in context");
                return;
            }

            _logger.LogInformation("Dispatching {EventCount} domain events", allEvents.Count);

            // Dispatch events in batches to optimize performance
            const int batchSize = 50;
            IEnumerable<IDomainEvent[]> batches = allEvents.Chunk(batchSize);

            foreach (var batch in batches)
            {
                var batchResult = await _eventDispatcher.PublishBatchAsync(batch, cancellationToken);

                if (batchResult.FailedEvents == 0)
                {
                    _logger.LogDebug(
                        "Successfully dispatched batch of {BatchSize} events in {ProcessingTime}ms",
                        batch.Count(),
                        batchResult.ProcessingTimeMs);
                }
                else
                {
                    _logger.LogWarning(
                        "Batch dispatch completed with {FailedEvents} failures out of {TotalEvents} events",
                        batchResult.FailedEvents,
                        batchResult.TotalEvents);
                }
            }

            // Clear all events after successful dispatch
            var clearedCount = domainEventsContext.ClearAllEvents();
            _logger.LogInformation("Cleared {ClearedCount} domain events after successful dispatch", clearedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while dispatching domain events");
            throw;
        }
    }

    /// <summary>
    /// Clears all domain events from the current context.
    /// </summary>
    /// <param name="context">The database context.</param>
    private void ClearDomainEvents(DbContext context)
    {
        try
        {
            var domainEventsContext = EntityEventExtensions.CurrentContext;
            if (domainEventsContext?.HasPendingEvents == true)
            {
                var clearedCount = domainEventsContext.ClearAllEvents();
                _logger.LogInformation("Cleared {ClearedCount} domain events due to save failure", clearedCount);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while clearing domain events");
            // Don't throw here as we're already handling a save failure
        }
    }
}