using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models.Repository;

public class EfOrderRepository(StoreDbContext context) : IOrderRepository
{
    private readonly StoreDbContext context = context ?? throw new ArgumentNullException(nameof(context));

    public IQueryable<Order> Orders => this.context.Orders
        .Include(o => o.Lines)
        .ThenInclude(l => l.Product);

    public void SaveOrder(Order order)
    {
        this.context.AttachRange(order.Lines.Select(l => l.Product));
        if (order.OrderId == 0)
        {
            this.context.Orders.Add(order);
        }
        this.context.SaveChanges();
    }
}
