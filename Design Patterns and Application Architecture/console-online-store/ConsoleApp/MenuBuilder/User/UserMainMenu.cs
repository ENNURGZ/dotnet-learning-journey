using ConsoleApp1;
using StoreDAL.Data;

namespace ConsoleMenu.Builder;

public class UserMainMenu : AbstractMenuCreator
{
    public override (ConsoleKey id, string caption, Action action)[] GetMenuItems(StoreDbContext context)
    {
        (ConsoleKey id, string caption, Action action)[] array =
            {
                (ConsoleKey.F1, "Logout", UserMenuController.Logout),
                (ConsoleKey.F2, "Show product list", ConsoleApp.Controllers.ProductController.ShowAllProducts),
                (ConsoleKey.F3, "Show order list", ConsoleApp.Services.ShopController.ShowAllOrders),
                (ConsoleKey.F4, "Cancel order", ConsoleApp.Services.ShopController.ProcessOrder),
                (ConsoleKey.F5, "Confirm order delivery", ConsoleApp.Services.ShopController.ProcessOrder),
                (ConsoleKey.F6, "Add order feedback", () => { Console.WriteLine("Add order feedback"); }),
                (ConsoleKey.F7, "Place order", ConsoleApp.Services.ShopController.AddOrder),
                (ConsoleKey.F8, "Update personal data", ConsoleApp.Services.UserController.UpdateUser),
            };
        return array;
    }
}