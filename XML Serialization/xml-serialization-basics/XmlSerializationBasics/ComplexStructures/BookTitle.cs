using System.Xml.Serialization;

namespace XmlSerializationBasics.ComplexStructures;

public class BookTitle
{
    [XmlElement("text")]
    public string? Title { get; set; }

    [XmlAttribute("language")]
    public string? Language { get; set; }
}
