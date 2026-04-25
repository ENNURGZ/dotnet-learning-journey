namespace OrderProcessingWithDi.Models;

/// <summary>
/// Standard error response model following RFC 7807 Problem Details.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Gets or sets the error type URI.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the HTTP status code.
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Gets or sets the error detail message.
    /// </summary>
    public string Detail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the instance URI where the error occurred.
    /// </summary>
    public string? Instance { get; set; }

    /// <summary>
    /// Gets or sets additional error extensions.
    /// </summary>
    public Dictionary<string, object> Extensions { get; set; } = new();
}



