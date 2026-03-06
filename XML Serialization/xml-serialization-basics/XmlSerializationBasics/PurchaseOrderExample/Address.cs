using System.Xml.Serialization;

namespace XmlSerializationBasics.PurchaseOrderExample;

public class Address
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlElement("ship-to-line1", Order = 2)]
    public string? Line1 { get; set; }

    [XmlElement("ship-to-city", Order = 1)]
    public string? City { get; set; }

    [XmlAttribute("ship-to-state")]
    public string? State { get; set; }

    [XmlAttribute("zip")]
    public string? Zip { get; set; }
}
