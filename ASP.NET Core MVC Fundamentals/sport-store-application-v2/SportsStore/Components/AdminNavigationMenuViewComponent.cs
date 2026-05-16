using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Components;

public class AdminNavigationMenuViewComponent : ViewComponent
{
    private static readonly string[] selections = { "Orders", "Products" };

    public IViewComponentResult Invoke()
    {
        this.ViewBag.Selection = this.Request.Path.Value ?? "Products";
        return this.View(selections);
    }
}
