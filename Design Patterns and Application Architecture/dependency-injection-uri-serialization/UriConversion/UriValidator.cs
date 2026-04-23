using LogerExtensionDelegate;
using Microsoft.Extensions.Logging;
using Validation;

namespace UriConversion;

/// <summary>
/// Uri string validator.
/// </summary>
public class UriValidator : IValidator<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UriValidator"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public UriValidator(ILogger<UriValidator>? logger = default)
    {
    }

    /// <summary>
    /// Determines if a string is valid Uri.
    /// </summary>
    /// <param name="obj">The source string.</param>
    /// <returns>true if the uri string is valid; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Throw if source string is null.</exception>
    public bool IsValid(string? obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        if (Uri.TryCreate(obj, UriKind.Absolute, out var uri))
        {
            return !string.IsNullOrEmpty(uri.Scheme) && !string.IsNullOrEmpty(uri.Host);
        }

        return false;
    }
}
