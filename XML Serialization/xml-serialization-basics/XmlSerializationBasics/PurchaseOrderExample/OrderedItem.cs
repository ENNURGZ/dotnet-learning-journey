using System.Xml.Serialization;

namespace XmlSerializationBasics.PurchaseOrderExample;

public class OrderedItem
{
    [XmlElement("order-item-name", Order = 1)]
    public string? ItemName { get; set; }

    [XmlElement("order-item-description", Order = 2)]
    public string? Description { get; set; }

    [XmlAttribute("unit-price")]
    public decimal UnitPrice { get; set; }

    [XmlAttribute("quantity")]
    public int Quantity { get; set; }

    [XmlIgnore]
    public decimal LineTotal { get; set; }

    public void CalculateLineTotal()
    {
        this.LineTotal = this.UnitPrice * this.Quantity;
    }
}
