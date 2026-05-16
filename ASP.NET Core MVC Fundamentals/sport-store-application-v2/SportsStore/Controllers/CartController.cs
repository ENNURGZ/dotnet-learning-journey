using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.Repository;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers;

public class CartController(IStoreRepository repository, Cart cart) : Controller
{
    private readonly IStoreRepository repository = repository ?? throw new ArgumentNullException(nameof(repository));

    private readonly Cart cart = cart ?? throw new ArgumentNullException(nameof(cart));

    [HttpGet]
    public IActionResult Index(string? returnUrl)
    {
        return this.View(new CartViewModel
        {
            ReturnUrl = new Uri(returnUrl ?? "/", UriKind.Relative),
            Cart = this.cart
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(long productId, string? returnUrl)
    {
        Product? product = this.repository.Products.FirstOrDefault(p => p.ProductId == productId);

        if (product != null)
        {
            this.cart.AddItem(product, 1);

            return this.View(new CartViewModel 
            {
                Cart = this.cart, 
                ReturnUrl = new Uri(returnUrl ?? "/", UriKind.Relative)
            });
        }

        return this.RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Route("Cart/Remove")]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(long productId, string? returnUrl)
    {
        var lineToRemove = this.cart.Lines.FirstOrDefault(cl => cl.Product.ProductId == productId);
        if (lineToRemove != null)
        {
            this.cart.RemoveLine(lineToRemove.Product);
        }
        
        return this.View("Index", new CartViewModel
        {
            Cart = this.cart,
            ReturnUrl = new Uri(returnUrl ?? "/", UriKind.Relative)
        });
    }
}
