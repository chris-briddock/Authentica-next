

using Authentica.Service.Identity.Domain.Results;

namespace Authentica.Service.Identity.Domain.Contracts;

/// <summary>
/// Defines a contract for dispatching domain events within the system.
/// This interface provides mechanisms for publishing, processing, and managing domain events
/// in a thread-safe and performant manner.
/// </summary>
/// <typeparam name="TEvent">The type of domain event to be dispatched. Must implement <see cref="IDomainEvent"/>.</typeparam>
public interface IDomainEventDispatcher<TEvent> where TEvent : IDomainEvent
{
    /// <summary>
    /// Gets the read-only collection of unprocessed domain events.
    /// </summary>
    IReadOnlyCollection<TEvent> PendingEvents { get; }

    /// <summary>
    /// Gets the total count of events that have been processed since the dispatcher was created.
    /// </summary>
    int ProcessedEventCount { get; }

    /// <summary>
    /// Publishes a single domain event asynchronously.
    /// </summary>
    /// <param name="event">The domain event to publish.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the processing result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="event"/> is null.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    Task<DispatchResult> PublishAsync(TEvent @event,
                                      CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes multiple domain events asynchronously in an optimized batch operation.
    /// </summary>
    /// <param name="events">The collection of domain events to publish.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the batch processing result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="events"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="events"/> contains null elements.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    Task<BatchDispatchResult> PublishBatchAsync(IEnumerable<TEvent> events,
                                                CancellationToken cancellationToken = default);

    /// <summary>
    /// Processes all pending events asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the processing summary.</returns>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    Task<EventProcessingSummaryResult> ProcessPendingEventsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears all pending events from the dispatcher.
    /// </summary>
    /// <returns>The number of events that were cleared.</returns>
    int ClearPendingEvents();

    /// <summary>
    /// Registers an event handler for specific event types.
    /// </summary>
    /// <typeparam name="THandlerEvent">The type of event to handle.</typeparam>
    /// <param name="handler">The event handler delegate.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="handler"/> is null.</exception>
    void RegisterHandler<THandlerEvent>(Func<THandlerEvent, CancellationToken, Task> handler) where THandlerEvent : TEvent;
}