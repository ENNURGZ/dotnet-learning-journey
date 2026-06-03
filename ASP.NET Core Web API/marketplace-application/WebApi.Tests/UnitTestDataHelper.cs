using Data.Data;
using Data.Entities;

namespace WebApi.Tests;

internal static class UnitTestDataHelper
{
    public static void SeedData(TradeMarketDbContext context)
    {
        // Clear existing data first
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Add Persons first (base entities)
        var persons = new[]
        {
            new Person { Id = 1, Name = "Han", Surname = "Solo", BirthDate = new DateTime(1942, 7, 13) },
            new Person { Id = 2, Name = "Ethan", Surname = "Hunt", BirthDate = new DateTime(1964, 8, 18) }
        };
        context.Persons.AddRange(persons);
        context.SaveChanges();

        // Add Customers (depends on Persons)
        var customers = new[]
        {
            new Customer { Id = 1, PersonId = 1, DiscountValue = 20 },
            new Customer { Id = 2, PersonId = 2, DiscountValue = 10 }
        };
        context.Customers.AddRange(customers);
        context.SaveChanges();

        // Add Product Categories (independent)
        var categories = new[]
        {
            new ProductCategory { Id = 1, CategoryName = "Dairy products" },
            new ProductCategory { Id = 2, CategoryName = "Fruit juices" }
        };
        context.ProductCategories.AddRange(categories);
        context.SaveChanges();

        // Add Products (depends on Categories)
        var products = new[]
        {
            new Product { Id = 1, ProductCategoryId = 1, ProductName = "Milk", Price = 40 },
            new Product { Id = 2, ProductCategoryId = 2, ProductName = "Orange juice", Price = 20 }
        };
        context.Products.AddRange(products);
        context.SaveChanges();

        // Add Receipts (depends on Customers)
        var receipts = new[]
        {
            new Receipt { Id = 1, CustomerId = 1, OperationDate = new DateTime(2021, 7, 5), IsCheckedOut = true },
            new Receipt { Id = 2, CustomerId = 1, OperationDate = new DateTime(2021, 8, 10), IsCheckedOut = true },
            new Receipt { Id = 3, CustomerId = 2, OperationDate = new DateTime(2021, 10, 15), IsCheckedOut = false }
        };
        context.Receipts.AddRange(receipts);
        context.SaveChanges();

        // Add Receipt Details (depends on Receipts and Products)
        var receiptDetails = new[]
        {
            new ReceiptDetail { Id = 1, ReceiptId = 1, ProductId = 1, UnitPrice = 40, DiscountUnitPrice = 32, Quantity = 3 },
            new ReceiptDetail { Id = 2, ReceiptId = 1, ProductId = 2, UnitPrice = 20, DiscountUnitPrice = 16, Quantity = 1 },
            new ReceiptDetail { Id = 3, ReceiptId = 2, ProductId = 2, UnitPrice = 20, DiscountUnitPrice = 32, Quantity = 2 },
            new ReceiptDetail { Id = 4, ReceiptId = 3, ProductId = 1, UnitPrice = 40, DiscountUnitPrice = 36, Quantity = 2 },
            new ReceiptDetail { Id = 5, ReceiptId = 3, ProductId = 2, UnitPrice = 20, DiscountUnitPrice = 18, Quantity = 5 }
        };
        context.ReceiptsDetails.AddRange(receiptDetails);
        context.SaveChanges();
    }
}
