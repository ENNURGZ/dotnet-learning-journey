using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.Repository;

namespace SportsStore.Controllers;

public class OrderController(IOrderRepository orderRepository, Cart cart) : Controller
{
    private readonly IOrderRepository orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    private readonly Cart cart = cart ?? throw new ArgumentNullException(nameof(cart));

    [HttpGet]
    public ViewResult Checkout() => this.View(new Order());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Checkout(Order order)
    {
        if (!this.cart.Lines.Any())
        {
            this.ModelState.AddModelError("", "Sorry, your cart is empty!");
        }

        if (this.ModelState.IsValid)
        {
            order.SetLines(this.cart.Lines);
            this.orderRepository.SaveOrder(order);
            this.cart.Clear();
            return this.View("Completed", order.OrderId);
        }

        return this.View();
    }
}
