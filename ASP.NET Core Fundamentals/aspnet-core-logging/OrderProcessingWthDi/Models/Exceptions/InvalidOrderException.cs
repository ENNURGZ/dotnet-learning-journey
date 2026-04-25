namespace OrderProcessingWithDi.Models.Exceptions;

/// <summary>
/// Exception thrown when order validation fails.
/// </summary>
public class InvalidOrderException : Exception
{
    /// <summary>
    /// Initializes a new instance of the InvalidOrderException class.
    /// </summary>
    public InvalidOrderException()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the InvalidOrderException class.
    /// </summary>
    /// <param name="message">The validation error message.</param>
    public InvalidOrderException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the InvalidOrderException class.
    /// </summary>
    /// <param name="message">The validation error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public InvalidOrderException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

