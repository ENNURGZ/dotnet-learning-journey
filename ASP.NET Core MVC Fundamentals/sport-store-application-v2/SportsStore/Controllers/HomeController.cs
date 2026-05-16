using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.Repository;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers;

public class HomeController(IStoreRepository repository) : Controller
{
    private readonly int PageSize = 4;

    // NEW: Update Index action to support category filtering and pagination
    [HttpGet]
    public ViewResult Index(string? category, int productPage = 1)
    {
        if (productPage < 1)
        {
            productPage = 1;
        }

        return this.View(new ProductsListViewModel
        {
            Products = repository.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductId)
                .Skip((productPage - 1) * this.PageSize)
                .Take(this.PageSize),
            PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = this.PageSize,
                TotalItems = category == null ? repository.Products.Count() : repository.Products.Count(e => e.Category == category),
            },
            CurrentCategory = category,
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpGet]
    public IActionResult Error() => this.View();
}
