using Microsoft.AspNetCore.Identity;

namespace Authentica.Service.Identity.Domain.Results;

/// <summary>
/// Represents a summary of event processing operations.
/// </summary>
public sealed class EventProcessingSummaryResult : ResultBase<EventProcessingSummaryResult,IdentityError>
{
    /// <summary>
    /// Gets the number of events processed.
    /// </summary>
    public int ProcessedCount { get; }

    /// <summary>
    /// Gets the number of events that failed processing.
    /// </summary>
    public int FailedCount { get; }

    /// <summary>
    /// Gets the total processing time in milliseconds.
    /// </summary>
    public long TotalProcessingTimeMs { get; }

    /// <summary>
    /// Gets the average processing time per event in milliseconds.
    /// </summary>
    public double AverageProcessingTimeMs { get; }

    public EventProcessingSummaryResult(int processedCount,
                                        int failedCount,
                                        long totalProcessingTimeMs,
                                        double averageProcessingTimeMs)
    {
        ProcessedCount = processedCount;
        FailedCount = failedCount;
        TotalProcessingTimeMs = totalProcessingTimeMs;
        AverageProcessingTimeMs = averageProcessingTimeMs;
    }

    public EventProcessingSummaryResult(){}
}