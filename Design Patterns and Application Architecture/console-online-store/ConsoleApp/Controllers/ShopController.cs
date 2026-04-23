using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp.Controllers;
using ConsoleApp.Handlers.ContextMenuHandlers;
using ConsoleApp.Helpers;
using ConsoleApp1;
using ConsoleMenu;
using StoreBLL.Models;
using StoreBLL.Services;
using StoreDAL.Data;

namespace ConsoleApp.Services
{
    public static class ShopController
    {
        private static StoreDbContext context = UserMenuController.Context;

        public static void AddOrder()
        {
            Console.WriteLine("--- Place Order ---");
            Console.Write("Enter Product ID to order: ");
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                var productService = new ProductService(context);
                var product = productService.GetById(productId) as ProductModel;
                if (product != null)
                {
                    Console.Write("Enter quantity: ");
                    if (int.TryParse(Console.ReadLine(), out int amount))
                    {
                        var orderService = new CustomerOrderService(context);
                        var order = new CustomerOrderModel(0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture), UserMenuController.CurrentUserId, 1);
                        orderService.Add(order);

                        var addedOrder = orderService.GetAll().Cast<CustomerOrderModel>().Last();

                        var detailService = new OrderDetailService(context);
                        detailService.Add(new OrderDetailModel(0, addedOrder.Id, product.Id, product.UnitPrice, amount));

                        Console.WriteLine("Order placed successfully!");
                    }
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }

            Console.ReadKey();
        }

        public static void UpdateOrder()
        {
            if (UserMenuController.CurrentUserRole != UserRoles.Administrator)
            {
                Console.WriteLine("Only administrators can change order status manually.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Order ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                var service = new CustomerOrderService(context);
                var order = service.GetById(orderId) as CustomerOrderModel;
                if (order != null)
                {
                    Console.Write("Enter new Order State ID: ");
                    if (int.TryParse(Console.ReadLine(), out int stateId))
                    {
                        order.OrderStateId = stateId;
                        service.Update(order);
                        Console.WriteLine("Order status updated.");
                    }
                }
                else
                {
                    Console.WriteLine("Order not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }

            Console.ReadKey();
        }

        public static void ShowAllOrders()
        {
            Console.WriteLine("--- My Orders ---");
            var service = new CustomerOrderService(context);
            var stateService = new OrderStateService(context);
            var states = stateService.GetAll().Cast<OrderStateModel>().ToDictionary(s => s.Id);

            IEnumerable<CustomerOrderModel> orders;
            if (UserMenuController.CurrentUserRole == UserRoles.Administrator)
            {
                orders = service.GetAll().Cast<CustomerOrderModel>().ToList();
            }
            else
            {
                orders = service.GetAll().Cast<CustomerOrderModel>()
                    .Where(o => o.UserId == UserMenuController.CurrentUserId).ToList();
            }

            if (!orders.Any())
            {
                Console.WriteLine("You have no orders.");
            }
            else
            {
                foreach (var o in orders)
                {
                    var stateName = states.ContainsKey(o.OrderStateId) ? states[o.OrderStateId].StateName : "Unknown";
                    Console.WriteLine($"Order ID: {o.Id}, Time: {o.OperationTime}, Status: {stateName}");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public static void ProcessOrder()
        {
            Console.Write("Enter Order ID to process: ");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                var service = new CustomerOrderService(context);
                var order = service.GetById(orderId) as CustomerOrderModel;
                if (order != null && order.UserId == UserMenuController.CurrentUserId)
                {
                    Console.WriteLine("1. Cancel Order (only if New Order)");
                    Console.WriteLine("2. Confirm Delivery (only if Delivered to client)");
                    var choice = Console.ReadLine();
                    if (choice == "1" && order.OrderStateId == 1)
                    {
                        order.OrderStateId = 2;
                        service.Update(order);
                        Console.WriteLine("Order cancelled.");
                    }
                    else if (choice == "2" && order.OrderStateId == 7)
                    {
                        order.OrderStateId = 8;
                        service.Update(order);
                        Console.WriteLine("Delivery confirmed.");
                    }
                    else
                    {
                        Console.WriteLine("Action not allowed for current order state.");
                    }
                }
                else
                {
                    Console.WriteLine("Order not found or not yours.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }

            Console.ReadKey();
        }

        public static void ShowAllOrderStates()
        {
            var service = new OrderStateService(context);
            var menu = new ContextMenu(new AdminContextMenuHandler(service, InputHelper.ReadOrderStateModel), service.GetAll);
            menu.Run();
        }
    }
}
