using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace UriSerializationHelper;

[XmlRoot("uriAdresses")]
public class UriAddress
{
    [XmlElement("scheme")]
    public InnerNamed AttrScheme;

    [XmlElement("host")]
    public InnerNamed AttrHost;

    [JsonPropertyName("scheme")]
    [XmlIgnore]
    public string Scheme
    {
        get => this.AttrScheme.Name;
        set => this.AttrScheme.Name = value;
    }

    [JsonPropertyName("host")]
    [XmlIgnore]
    public string Host
    {
        get => this.AttrHost.Name;
        set => this.AttrHost.Name = value;
    }

    [JsonPropertyName("path")]
    [XmlArray("path")]
    [XmlArrayItem("segment")]
    public List<string> Path { get; set; } = null!;

    [JsonIgnore]
    [XmlIgnore]
    public List<QueryElement> Query { get; set; } = null!;

    [JsonPropertyName("query")]
    [XmlArray("query")]
    [XmlArrayItem("parameter")]
    public List<QueryElement>? QuerySerializable
    {
        get => this.Query.Count > 0 ? this.Query : null;
    }

    public struct InnerNamed
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
