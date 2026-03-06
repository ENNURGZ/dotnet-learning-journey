using System.Xml.Serialization;

namespace XmlSerializationBasics.PurchaseOrderExample;

public class DeliveryDate
{
    [XmlAttribute("day")]
    public int DeliveryDay { get; set; }

    [XmlAttribute("month")]
    public int DeliveryMonth { get; set; }

    [XmlElement("year")]
    public int DeliveryYear { get; set; }
}
