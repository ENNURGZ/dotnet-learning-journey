namespace OrderProcessingWithDi.Models.Exceptions;

/// <summary>
/// Exception thrown when order validation fails.
/// TODO: Implement this exception class.
/// 
/// Requirements:
/// - Inherit from Exception
/// - Add constructor: InvalidOrderException(string message)
///   - Should pass message to base Exception constructor
/// - Include standard Exception constructors for CA1032 compliance:
///   - InvalidOrderException()
///   - InvalidOrderException(string message)
///   - InvalidOrderException(string message, Exception innerException)
/// </summary>
public class InvalidOrderException : Exception
{
    public InvalidOrderException()
        : base()
    {
    }
    
    public InvalidOrderException(string message)
        : base(message)
    {
    }
    
    public InvalidOrderException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

