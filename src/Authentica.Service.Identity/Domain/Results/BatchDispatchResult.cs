using Microsoft.AspNetCore.Identity;

namespace Authentica.Service.Identity.Domain.Results;

/// <summary>
/// Represents the result of a batch event dispatch operation.
/// </summary>
public sealed class BatchDispatchResult : ResultBase<BatchDispatchResult, IdentityError>
{
    /// <summary>
    /// Gets the total number of events processed.
    /// </summary>
    public int TotalEvents { get; }

    /// <summary>
    /// Gets the number of successfully processed events.
    /// </summary>
    public int SuccessfulEvents { get; }

    /// <summary>
    /// Gets the number of failed events.
    /// </summary>
    public int FailedEvents { get; }

    /// <summary>
    /// Gets the processing time in milliseconds.
    /// </summary>
    public long ProcessingTimeMs { get; }

    /// <summary>
    /// Gets the individual results for each event.
    /// </summary>
    public IReadOnlyList<DispatchResult> IndividualResults { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BatchDispatchResult"/>
    /// </summary>
    /// <param name="totalEvents">The total number of events.</param>
    /// <param name="successfulEvents">The number of successful events.</param>
    /// <param name="failedEvents">The number of failed events.</param>
    /// <param name="processingTimeMs">The processing time in milliseconds.</param>
    /// <param name="individualResults">The individual results for each event.</param>
    public BatchDispatchResult(int totalEvents, int successfulEvents, int failedEvents, long processingTimeMs, IReadOnlyList<DispatchResult> individualResults)
    {
        TotalEvents = totalEvents;
        SuccessfulEvents = successfulEvents;
        FailedEvents = failedEvents;
        ProcessingTimeMs = processingTimeMs;
        IndividualResults = individualResults;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="BatchDispatchResult"/>
    /// </summary>
    public BatchDispatchResult()
    {
        
    }
    /// <summary>
    /// Gets the success rate as a percentage.
    /// </summary>
    public double SuccessRate => TotalEvents > 0 ? (double)SuccessfulEvents / TotalEvents * 100 : 0;
}