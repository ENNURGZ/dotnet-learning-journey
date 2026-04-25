namespace OrderProcessingWithDi.Models;

/// <summary>
/// Standard error response model following RFC 7807 Problem Details.
/// TODO: Implement this class.
/// 
/// Requirements:
/// Add the following properties:
/// - Type (string) - URI reference identifying the problem type
/// - Title (string) - Short summary of the problem
/// - Status (int) - HTTP status code
/// - Detail (string) - Detailed explanation of the problem
/// - Instance (string?) - URI reference identifying the specific occurrence
/// - Extensions (Dictionary&lt;string, object&gt;) - Additional error information
/// 
/// All properties should have public getters and setters.
/// Initialize Extensions with new Dictionary&lt;string, object&gt;()
/// Initialize string properties with string.Empty
/// </summary>
public class ErrorResponse
{
    // TODO: Implement ErrorResponse properties
    // Properties are added below for compilation purposes only
    // Students need to implement them according to requirements
    
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
