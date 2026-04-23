using System.Xml;
using System.Xml.Serialization;
using LogerExtensionDelegate;
using Microsoft.Extensions.Logging;
using Serialization;
using UriSerializationHelper;

namespace XmlSerializer.Serialization;

/// <summary>
/// Presents the serialization functionality of the sequence<see cref="IEnumerable{Uri}"/>
/// with using XmlSerializer class.
/// </summary>
public class XmlSerializerTechnology : IDataSerializer<Uri>
{
    private readonly string path;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlSerializerTechnology"/> class.
    /// </summary>
    /// <param name="path">The path to json file.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentException">Throw if text reader is null or empty.</exception>
    public XmlSerializerTechnology(string? path, ILogger<XmlSerializerTechnology>? logger = default)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException("Path is null or empty.", nameof(path));
        }

        this.path = path;
    }

    /// <summary>
    /// Serializes the source sequence of Uri elements in json format.
    /// </summary>
    /// <param name="source">The source sequence of Uri elements.</param>
    /// <exception cref="ArgumentNullException">Throw if the source sequence is null.</exception>
    public void Serialize(IEnumerable<Uri>? source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var container = new UriContainer(source.Select(uri => uri.ToSerializableObject()));
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(UriContainer));
        var settings = new XmlWriterSettings { Indent = true };
        using var writer = XmlWriter.Create(this.path, settings);
        serializer.Serialize(writer, container);
    }
}
