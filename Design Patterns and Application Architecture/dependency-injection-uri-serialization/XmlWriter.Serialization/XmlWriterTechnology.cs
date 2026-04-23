using System.Xml;
using LogerExtensionDelegate;
using Microsoft.Extensions.Logging;
using Serialization;
using UriSerializationHelper;

namespace XmlWriter.Serialization;

/// <summary>
/// Presents the serialization functionality of the sequence<see cref="IEnumerable{Uri}"/>
/// with using XmlWriter class.
/// </summary>
public class XmlWriterTechnology : IDataSerializer<Uri>
{
    private readonly string path;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlWriterTechnology"/> class.
    /// </summary>
    /// <param name="path">The path to json file.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentException">Throw if text reader is null or empty.</exception>
    public XmlWriterTechnology(string? path, ILogger<XmlWriterTechnology>? logger = default)
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

        var settings = new XmlWriterSettings { Indent = true };
        using var writer = System.Xml.XmlWriter.Create(this.path, settings);
        writer.WriteStartDocument();
        writer.WriteStartElement("uriAdresses");

        foreach (var uri in source)
        {
            var obj = uri.ToSerializableObject();
            writer.WriteStartElement("uriAdress");

            writer.WriteStartElement("scheme");
            writer.WriteAttributeString("name", obj.Scheme);
            writer.WriteEndElement();

            writer.WriteStartElement("host");
            writer.WriteAttributeString("name", obj.Host);
            writer.WriteEndElement();

            writer.WriteStartElement("path");
            foreach (var segment in obj.Path)
            {
                writer.WriteElementString("segment", segment);
            }

            writer.WriteEndElement();

            if (obj.QuerySerializable != null)
            {
                writer.WriteStartElement("query");
                foreach (var param in obj.QuerySerializable)
                {
                    writer.WriteStartElement("parameter");
                    writer.WriteAttributeString("key", param.Key);
                    writer.WriteAttributeString("value", param.Value);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        writer.WriteEndElement();
        writer.WriteEndDocument();
    }
}
