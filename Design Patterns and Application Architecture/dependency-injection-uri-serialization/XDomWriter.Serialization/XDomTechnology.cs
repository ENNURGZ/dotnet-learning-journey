using System.Xml;
using System.Xml.Linq;
using LogerExtensionDelegate;
using Microsoft.Extensions.Logging;
using Serialization;
using UriSerializationHelper;

namespace XDomWriter.Serialization;

/// <summary>
/// Presents the serialization functionality of the sequence<see cref="IEnumerable{Uri}"/>
/// with using X-DOM model.
/// </summary>
public class XDomTechnology : IDataSerializer<Uri>
{
    private readonly string path;

    /// <summary>
    /// Initializes a new instance of the <see cref="XDomTechnology"/> class.
    /// </summary>
    /// <param name="path">The path to json file.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentException">Throw if text reader is null or empty.</exception>
    public XDomTechnology(string? path, ILogger<XDomTechnology>? logger = default)
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

        var doc = new XDocument(
            new XElement(
                "uriAdresses",
                source.Select(uri =>
                {
                    var obj = uri.ToSerializableObject();
                    return new XElement(
                        "uriAdress",
                        new XElement("scheme", new XAttribute("name", obj.Scheme)),
                        new XElement("host", new XAttribute("name", obj.Host)),
                        new XElement("path", obj.Path.Select(s => new XElement("segment", s))),
                        obj.QuerySerializable != null
                            ? new XElement(
                                "query",
                                obj.QuerySerializable.Select(p =>
                                    new XElement(
                                        "parameter",
                                        new XAttribute("key", p.Key),
                                        new XAttribute("value", p.Value))))
                            : null);
                })));

        doc.Save(this.path);
    }
}
