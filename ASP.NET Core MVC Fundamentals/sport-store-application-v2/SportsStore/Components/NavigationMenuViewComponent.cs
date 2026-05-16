using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.Repository;

namespace SportsStore.Components;

public class NavigationMenuViewComponent : ViewComponent
{
    private readonly IStoreRepository repository;

    public NavigationMenuViewComponent(IStoreRepository repository)
    {
        this.repository = repository;
    }

    public IViewComponentResult Invoke()
    {
        // NEW: Set selected category from route data using ViewBag
        ViewBag.SelectedCategory = RouteData?.Values["category"];
        
        // NEW: Return distinct categories using LINQ and ordered alphabetically
        return View(repository.Products
             .Select(x => x.Category)
             .Distinct()
             .OrderBy(x => x));
    }
}
