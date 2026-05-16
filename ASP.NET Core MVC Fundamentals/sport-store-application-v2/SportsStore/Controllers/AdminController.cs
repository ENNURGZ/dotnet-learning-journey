using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.Repository;

namespace SportsStore.Controllers;

[Authorize]
[Route("Admin")]
public class AdminController(IStoreRepository storeRepository, IOrderRepository orderRepository) : Controller
{
    private readonly IStoreRepository storeRepository = storeRepository ?? throw new ArgumentNullException(nameof(storeRepository));
    private readonly IOrderRepository orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));

    [HttpGet]
    [Route("Orders")]
    public ViewResult Orders() => this.View(this.orderRepository.Orders);

    [HttpGet]
    [Route("Products")]
    public ViewResult Products() => this.View(this.storeRepository.Products);

    [HttpPost]
    [Route("MarkShipped")]
    [ValidateAntiForgeryToken]
    public IActionResult MarkShipped(int orderId)
    {
        Order? order = this.orderRepository.Orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order != null)
        {
            order.Shipped = true;
            this.orderRepository.SaveOrder(order);
        }
        return this.RedirectToAction(nameof(this.Orders));
    }

    [HttpPost]
    [Route("Reset")]
    [ValidateAntiForgeryToken]
    public IActionResult Reset(int orderId)
    {
        Order? order = this.orderRepository.Orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order != null)
        {
            order.Shipped = false;
            this.orderRepository.SaveOrder(order);
        }
        return this.RedirectToAction(nameof(this.Orders));
    }

    [HttpGet]
    [Route("Details/{productId:long}")]
    public ViewResult Details(long productId)
        => this.View(this.storeRepository.Products.FirstOrDefault(p => p.ProductId == productId));

    [Route("Edit/{productId:long}")]
    [HttpGet]
    public ViewResult Edit(long productId)
        => this.View(this.storeRepository.Products.FirstOrDefault(p => p.ProductId == productId));

    [HttpPost]
    [Route("Edit")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Product product)
    {
        if (this.ModelState.IsValid)
        {
            this.storeRepository.SaveProduct(product);
            return this.RedirectToAction(nameof(this.Products));
        }

        return this.View(product);
    }

    [Route("Create")]
    [HttpGet]
    public ViewResult Create() => this.View(new Product());

    [HttpPost]
    [Route("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product)
    {
        if (this.ModelState.IsValid)
        {
            this.storeRepository.SaveProduct(product);
            return this.RedirectToAction(nameof(this.Products));
        }

        return this.View(product);
    }

    [Route("Delete/{productId:long}")]
    [HttpGet]
    public ViewResult Delete(long productId)
        => this.View(this.storeRepository.Products.FirstOrDefault(p => p.ProductId == productId));

    [HttpPost]
    [Route("Delete/{productId:long}")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteProduct(long productId)
    {
        Product? product = this.storeRepository.Products.FirstOrDefault(p => p.ProductId == productId);
        if (product != null)
        {
            this.storeRepository.DeleteProduct(product);
        }
        return this.RedirectToAction(nameof(this.Products));
    }
}
