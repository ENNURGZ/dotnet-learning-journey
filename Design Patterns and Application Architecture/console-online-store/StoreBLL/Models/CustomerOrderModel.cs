namespace StoreBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CustomerOrderModel : AbstractModel
{
    public CustomerOrderModel(int id, string operationTime, int userId, int orderStateId)
        : base(id)
    {
        this.OperationTime = operationTime;
        this.UserId = userId;
        this.OrderStateId = orderStateId;
    }

    public string OperationTime { get; set; }

    public int UserId { get; set; }

    public int OrderStateId { get; set; }
}
