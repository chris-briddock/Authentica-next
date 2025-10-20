namespace Authentica.Service.Identity.Domain.Results;

public abstract class ResultBase<TResult, TError> 
where TResult : ResultBase<TResult, TError>, new()
where TError : class

{
    /// <summary>
    /// Gets an enumeration of errors associated with the result of an operation.
    /// </summary>
    public IList<TError> Errors = [];
     /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    public bool IsSuccess { get; private set; }

    /// <summary>
    /// Creates a new result indicating the success of an operation.
    /// </summary>
    /// <returns>A new successful result.</returns>
    public static TResult Success()
    {
        return new TResult()
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a new result indicating a failed operation with the specified errors.
    /// </summary>
    /// <param name="errors">An array of <see cref="IdentityError"/> objects representing the errors.</param>
    /// <returns>A new result indicating a failed operation.</returns>
    public static TResult Failed(params TError[] errors)
    {
        var result = new TResult { IsSuccess = false };
        
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