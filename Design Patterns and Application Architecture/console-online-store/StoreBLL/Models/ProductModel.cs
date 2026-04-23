namespace StoreBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProductModel : AbstractModel
{
    public ProductModel(int id, int titleId, int manufacturerId, string description, decimal price)
        : base(id)
    {
        this.TitleId = titleId;
        this.ManufacturerId = manufacturerId;
        this.Description = description;
        this.UnitPrice = price;
    }

    public int TitleId { get; set; }

    public int ManufacturerId { get; set; }

    public decimal UnitPrice { get; set; }

    public string Description { get; set; }
}
