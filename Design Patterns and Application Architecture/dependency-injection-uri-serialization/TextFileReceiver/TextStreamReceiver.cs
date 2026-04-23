using DataReceiving;
using Microsoft.Extensions.Logging;

namespace TextFileReceiver;

/// <summary>
/// The data receiver from text file.
/// </summary>
public class TextStreamReceiver : IDataReceiver
{
    private readonly string path;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextStreamReceiver"/> class.
    /// </summary>
    /// <param name="path">The path to text file.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentException">Throw if text reader is null or empty.</exception>
    public TextStreamReceiver(string? path, ILogger<TextStreamReceiver>? logger = default)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException("Path is null or empty.", nameof(path));
        }

        this.path = path;
    }

    /// <summary>
    /// Receives lines from text reader.
    /// </summary>
    /// <returns>Strings.</returns>
    public IEnumerable<string> Receive()
    {
        return File.ReadLines(this.path);
    }
}
