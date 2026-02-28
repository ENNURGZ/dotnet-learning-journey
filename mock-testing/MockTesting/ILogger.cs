namespace MockTesting;

/// <summary>
/// Provides functionality for logging messages and events.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Logs a message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void Log(string message);
}
