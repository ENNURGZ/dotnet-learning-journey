using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1;
using StoreDAL.Data;

namespace ConsoleApp.Controllers
{
    public static class ProductController
    {
        private static StoreDbContext context = UserMenuController.Context;

        public static void AddProduct()
        {
            Console.WriteLine("--- Add New Product ---");
            Console.Write("Title ID: ");
            if (int.TryParse(Console.ReadLine(), out int titleId))
            {
                Console.Write("Manufacturer ID: ");
                if (int.TryParse(Console.ReadLine(), out int manId))
                {
                    Console.Write("Price: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal price))
                    {
                        Console.Write("Description: ");
                        var desc = Console.ReadLine();

                        var service = new StoreBLL.Services.ProductService(context);
                        service.Add(new StoreBLL.Models.ProductModel(0, titleId, manId, desc ?? string.Empty, price));
                        Console.WriteLine("Product added successfully!");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }

            Console.ReadKey();
        }

        public static void UpdateProduct()
        {
            Console.Write("Enter Product ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var service = new StoreBLL.Services.ProductService(context);
                var product = service.GetById(id) as StoreBLL.Models.ProductModel;
                if (product != null)
                {
                    Console.WriteLine($"Current Price: {product.UnitPrice}");
                    Console.Write("New Price (leave empty to keep): ");
                    var priceStr = Console.ReadLine();
                    if (decimal.TryParse(priceStr, out decimal price))
                    {
                        product.UnitPrice = price;
                    }

                    Console.WriteLine($"Current Description: {product.Description}");
                    Console.Write("New Description (leave empty to keep): ");
                    var desc = Console.ReadLine();
                    if (!string.IsNullOrEmpty(desc))
                    {
                        product.Description = desc;
                    }

                    service.Update(product);
                    Console.WriteLine("Product updated.");
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }
            }

            Console.ReadKey();
        }

        public static void DeleteProduct()
        {
            Console.Write("Enter Product ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var service = new StoreBLL.Services.ProductService(context);
                service.Delete(id);
                Console.WriteLine("Product deleted.");
            }

            Console.ReadKey();
        }

        public static void ShowProduct()
        {
            throw new NotImplementedException();
        }

        public static void ShowAllProducts()
        {
            Console.WriteLine("--- Product List ---");
            var productService = new StoreBLL.Services.ProductService(context);
            var titleService = new StoreBLL.Services.ProductTitleService(context);
            var manufacturerService = new StoreBLL.Services.ManufacturerService(context);

            var products = productService.GetAll().Cast<StoreBLL.Models.ProductModel>().ToList();
            var titles = titleService.GetAll().Cast<StoreBLL.Models.ProductTitleModel>().ToDictionary(t => t.Id);
            var manufacturers = manufacturerService.GetAll().Cast<StoreBLL.Models.ManufacturerModel>().ToDictionary(m => m.Id);

            Console.WriteLine($"{"ID",-5} | {"Product Name",-20} | {"Manufacturer",-15} | {"Price",-10} | {"Description"}");
            Console.WriteLine(new string('-', 80));

            foreach (var p in products)
            {
                var title = titles.ContainsKey(p.TitleId) ? titles[p.TitleId].Title : "Unknown";
                var manufacturer = manufacturers.ContainsKey(p.ManufacturerId) ? manufacturers[p.ManufacturerId].ManufacturerName : "Unknown";
                Console.WriteLine($"{p.Id,-5} | {title,-20} | {manufacturer,-15} | {p.UnitPrice,-10:C} | {p.Description}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public static void AddCategory()
        {
            throw new NotImplementedException();
        }

        public static void UpdateCategory()
        {
            throw new NotImplementedException();
        }

        public static void DeleteCategory()
        {
            throw new NotImplementedException();
        }

        public static void ShowAllCategories()
        {
            throw new NotImplementedException();
        }

        public static void AddProductTitle()
        {
            throw new NotImplementedException();
        }

        public static void UpdateProductTitle()
        {
            throw new NotImplementedException();
        }

        public static void DeleteProductTitle()
        {
            throw new NotImplementedException();
        }

        public static void ShowAllProductTitles()
        {
            throw new NotImplementedException();
        }

        public static void AddManufacturer()
        {
            throw new NotImplementedException();
        }

        public static void UpdateManufacturer()
        {
            throw new NotImplementedException();
        }

        public static void DeleteManufacturer()
        {
            throw new NotImplementedException();
        }

        public static void ShowAllManufacturers()
        {
            throw new NotImplementedException();
        }
    }
}