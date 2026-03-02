using System.Text.Json;
using System.Text.Json.Serialization;

[assembly: CLSCompliant(true)]

namespace JsonSerialization;

public static class JsonSerializationOperations
{
    public static string SerializeObjectToJson(object obj)
    {
        return JsonSerializer.Serialize(obj);
    }

    public static T? DeserializeJsonToObject<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json);
    }

    public static string SerializeCompanyObjectToJson(object obj)
    {
        return JsonSerializer.Serialize(obj);
    }

    public static T? DeserializeCompanyJsonToObject<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json);
    }

    public static string SerializeDictionary(Company obj)
    {
        var options = new JsonSerializerOptions
        {
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        };

        return JsonSerializer.Serialize(obj.Domains, options);
    }

    public static string SerializeEnum(Company obj)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

        return JsonSerializer.Serialize(obj, options);
    }
}
