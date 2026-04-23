using System.Xml;
using LogerExtensionDelegate;
using Microsoft.Extensions.Logging;
using Serialization;
using UriSerializationHelper;

namespace XmlDomWriter.Serialization;

/// <summary>
/// Presents the serialization functionality of the sequence<see cref="IEnumerable{Uri}"/>
/// with using XML-DOM model.
/// </summary>
public class XmlDomTechnology : IDataSerializer<Uri>
{
    private readonly string path;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlDomTechnology"/> class.
    /// </summary>
    /// <param name="path">The path to json file.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentException">Throw if text reader is null or empty.</exception>
    public XmlDomTechnology(string? path, ILogger<XmlDomTechnology>? logger = default)
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

        var doc = new XmlDocument();
        var root = doc.CreateElement("uriAdresses");
        doc.AppendChild(root);

        foreach (var uri in source)
        {
            var obj = uri.ToSerializableObject();
            var addressElement = doc.CreateElement("uriAdress");
            root.AppendChild(addressElement);

            var schemeElement = doc.CreateElement("scheme");
            schemeElement.SetAttribute("name", obj.Scheme);
            addressElement.AppendChild(schemeElement);

            var hostElement = doc.CreateElement("host");
            hostElement.SetAttribute("name", obj.Host);
            addressElement.AppendChild(hostElement);

            var pathElement = doc.CreateElement("path");
            addressElement.AppendChild(pathElement);
            foreach (var segment in obj.Path)
            {
                var segmentElement = doc.CreateElement("segment");
                segmentElement.InnerText = segment;
                pathElement.AppendChild(segmentElement);
            }

            if (obj.QuerySerializable != null)
            {
                var queryElement = doc.CreateElement("query");
                addressElement.AppendChild(queryElement);
                foreach (var param in obj.QuerySerializable)
                {
                    var parameterElement = doc.CreateElement("parameter");
                    parameterElement.SetAttribute("key", param.Key);
                    parameterElement.SetAttribute("value", param.Value);
                    queryElement.AppendChild(parameterElement);
                }
            }
        }

        doc.Save(this.path);
    }
}
