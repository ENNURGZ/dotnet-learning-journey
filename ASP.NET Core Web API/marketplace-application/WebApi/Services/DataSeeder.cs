using Data.Data;
using Data.Entities;

namespace WebApi.Services;

public class DataSeeder(TradeMarketDbContext db)
{
    public void Seed()
    {
        // Если база и таблицы не созданы, создать
        if (db.Database.EnsureCreated())
        {
            // Персоны и клиенты
            var persons = new[]
            {
                new Person { Name = "Ivan", Surname = "Ivanov", BirthDate = new DateTime(1990, 1, 1) },
                new Person { Name = "Anna", Surname = "Petrova", BirthDate = new DateTime(1985, 5, 15) },
                new Person { Name = "Sergey", Surname = "Sidorov", BirthDate = new DateTime(2000, 12, 31) }
            };
            db.Persons.AddRange(persons);
            _ = db.SaveChanges();

            var customers = new[]
            {
                new Customer { PersonId = persons[0].Id, DiscountValue = 10 },
                new Customer { PersonId = persons[1].Id, DiscountValue = 5 },
                new Customer { PersonId = persons[2].Id, DiscountValue = 0 }
            };
            db.Customers.AddRange(customers);
            _ = db.SaveChanges();

            var categories = new[]
            {
                new ProductCategory { CategoryName = "Food" },
                new ProductCategory { CategoryName = "Drinks" },
                new ProductCategory { CategoryName = "Household" }
            };
            db.ProductCategories.AddRange(categories);
            _ = db.SaveChanges();

            var products = new[]
            {
                new Product { ProductName = "Bread", Price = 50, ProductCategoryId = categories[0].Id },
                new Product { ProductName = "Milk", Price = 70, ProductCategoryId = categories[1].Id },
                new Product { ProductName = "Soap", Price = 30, ProductCategoryId = categories[2].Id },
                new Product { ProductName = "Cheese", Price = 200, ProductCategoryId = categories[0].Id }
            };
            db.Products.AddRange(products);
            _ = db.SaveChanges();

            var receipts = new[]
            {
                new Receipt { CustomerId = customers[0].Id, OperationDate = DateTime.Now.AddDays(-2), IsCheckedOut = true },
                new Receipt { CustomerId = customers[1].Id, OperationDate = DateTime.Now.AddDays(-1), IsCheckedOut = true },
                new Receipt { CustomerId = customers[2].Id, OperationDate = DateTime.Now, IsCheckedOut = false }
            };
            db.Receipts.AddRange(receipts);
            _ = db.SaveChanges();

            var receiptDetails = new[]
            {
                new ReceiptDetail { ReceiptId = receipts[0].Id, ProductId = products[0].Id, UnitPrice = 50, DiscountUnitPrice = 45, Quantity = 2 },
                new ReceiptDetail { ReceiptId = receipts[0].Id, ProductId = products[1].Id, UnitPrice = 70, DiscountUnitPrice = 65, Quantity = 1 },
                new ReceiptDetail { ReceiptId = receipts[1].Id, ProductId = products[2].Id, UnitPrice = 30, DiscountUnitPrice = 28, Quantity = 3 },
                new ReceiptDetail { ReceiptId = receipts[2].Id, ProductId = products[3].Id, UnitPrice = 200, DiscountUnitPrice = 180, Quantity = 1 }
            };
            db.ReceiptsDetails.AddRange(receiptDetails);
            _ = db.SaveChanges();
        }
    }
}
