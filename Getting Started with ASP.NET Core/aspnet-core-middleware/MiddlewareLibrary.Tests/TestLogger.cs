using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace MiddlewareLibrary.Tests;

internal sealed class TestLogger<T> : ILogger<T>
{
    private readonly ConcurrentQueue<LogEntry> _entries = new();

    public IReadOnlyCollection<LogEntry> Entries => _entries;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);
        _entries.Enqueue(new LogEntry(logLevel, message, exception));
    }

    public sealed record LogEntry(LogLevel Level, string Message, Exception? Exception);
}


