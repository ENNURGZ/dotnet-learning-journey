using ConsoleApp.Services;
using ConsoleApp1;
using StoreDAL.Data;

namespace ConsoleMenu.Builder;

public class AdminMainMenu : AbstractMenuCreator
{
    public override (ConsoleKey id, string caption, Action action)[] GetMenuItems(StoreDbContext context)
    {
        (ConsoleKey id, string caption, Action action)[] array =
            {
                (ConsoleKey.F1, "Logout", UserMenuController.Logout),
                (ConsoleKey.F2, "Show product list", ConsoleApp.Controllers.ProductController.ShowAllProducts),
                (ConsoleKey.F3, "Add product", ConsoleApp.Controllers.ProductController.AddProduct),
                (ConsoleKey.F4, "Show all orders", ConsoleApp.Services.ShopController.ShowAllOrders),
                (ConsoleKey.F5, "Change order status", ConsoleApp.Services.ShopController.UpdateOrder),
                (ConsoleKey.F6, "User roles", ConsoleApp.Services.UserController.ShowAllUserRoles),
                (ConsoleKey.F7, "Order states", ConsoleApp.Services.ShopController.ShowAllOrderStates),
                (ConsoleKey.F8, "Edit product", ConsoleApp.Controllers.ProductController.UpdateProduct),
                (ConsoleKey.F9, "Delete product", ConsoleApp.Controllers.ProductController.DeleteProduct),
            };
        return array;
    }
}