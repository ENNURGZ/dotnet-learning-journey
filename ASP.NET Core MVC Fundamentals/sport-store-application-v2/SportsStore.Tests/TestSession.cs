using Microsoft.AspNetCore.Http;

namespace SportsStore.Tests;

public class TestSession : ISession
{
    private readonly Dictionary<string, byte[]> sessionData = new();

    public string Id => "test-session-id";
    public bool IsAvailable => true;
    public IEnumerable<string> Keys => this.sessionData.Keys;

    public void Clear() => this.sessionData.Clear();

    public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public void Remove(string key) => this.sessionData.Remove(key);

    public void Set(string key, byte[] value) => this.sessionData[key] = value;

    public bool TryGetValue(string key, out byte[] value) => this.sessionData.TryGetValue(key, out value!);
}
