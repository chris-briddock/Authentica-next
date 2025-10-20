using Microsoft.AspNetCore.Identity;

namespace Authentica.Service.Identity.Domain.Results;

/// <summary>
/// Represents the result of a single event dispatch operation.
/// </summary>
public sealed class DispatchResult : ResultBase<DispatchResult, IdentityError>
{
    /// <summary>
    /// Gets the processing time in milliseconds.
    /// </summary>
    public long ProcessingTimeMs { get; private set; } = default!;

    /// <summary>
    /// Initializes a new instance of <see cref="DispatchResult"/>
    /// </summary>
    /// <param name="processingTimeMs"></param>
    public DispatchResult(long processingTimeMs)
    {
        ProcessingTimeMs = processingTimeMs;
    }

    public DispatchResult() { }

    /// <summary>
    /// Creates a successful dispatch result.
    /// </summary>
    /// <param name="processingTimeMs">The processing time in milliseconds.</param>
    /// <returns>A successful dispatch result.</returns>
    public static DispatchResult Success(long processingTimeMs) => new(processingTimeMs);

    /// <summary>
    /// Creates a failed dispatch result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="processingTimeMs">The processing time in milliseconds.</param>
    /// <returns>A failed dispatch result.</returns>
    public static DispatchResult Failure(long processingTimeMs, params IdentityError[] errors)
    {
        var result = new DispatchResult() { ProcessingTimeMs = processingTimeMs };

        if (errors is not null)
        {
            foreach (var error in errors)
            {
                result.Errors.Add(error);
            }    
        }

        return result;
    }
}