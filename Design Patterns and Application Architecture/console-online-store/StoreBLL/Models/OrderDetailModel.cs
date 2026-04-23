namespace StoreBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OrderDetailModel : AbstractModel
{
    public OrderDetailModel(int id, int orderId, int productId, decimal price, int productAmount)
        : base(id)
    {
        this.OrderId = orderId;
        this.ProductId = productId;
        this.Price = price;
        this.ProductAmount = productAmount;
    }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public decimal Price { get; set; }

    public int ProductAmount { get; set; }
}
