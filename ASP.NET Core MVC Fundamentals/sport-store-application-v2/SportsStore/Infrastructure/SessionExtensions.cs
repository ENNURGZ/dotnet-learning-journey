using System.Text.Json;
using System.Text.Json.Serialization;

namespace SportsStore.Infrastructure;

// NEW: Add SessionExtensions class for JSON serialization
public static class SessionExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate
    };

    public static void SetJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonSerializer.Serialize(value, SerializerOptions));
    }

    public static T? GetJson<T>(this ISession session, string key)
    {
        var sessionData = session.GetString(key);
        return string.IsNullOrEmpty(sessionData) ? default : JsonSerializer.Deserialize<T>(sessionData, SerializerOptions);
    }
}
