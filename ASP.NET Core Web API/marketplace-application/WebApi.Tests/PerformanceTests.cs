using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using Business.Models;
using NUnit.Framework;

namespace WebApi.Tests;

[TestFixture]
public class PerformanceTests
{
    private CustomWebApplicationFactory factory;
    private HttpClient client;

    [SetUp]
    public void SetUp()
    {
        this.factory = new CustomWebApplicationFactory();
        this.client = this.factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        this.client?.Dispose();
        this.factory?.Dispose();
    }

    [Test]
    public async Task GetAllCustomers_CompletesWithinAcceptableTime()
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await this.client.GetAsync(new Uri("/api/customers", UriKind.Relative));
        stopwatch.Stop();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(5000), "GetAllCustomers should complete within 5 seconds");
    }

    [Test]
    public async Task GetAllProducts_CompletesWithinAcceptableTime()
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await this.client.GetAsync(new Uri("/api/products", UriKind.Relative));
        stopwatch.Stop();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(5000), "GetAllProducts should complete within 5 seconds");
    }

    [Test]
    public async Task GetAllReceipts_CompletesWithinAcceptableTime()
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await this.client.GetAsync(new Uri("/api/receipts", UriKind.Relative));
        stopwatch.Stop();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(5000), "GetAllReceipts should complete within 5 seconds");
    }

    [Test]
    public async Task GetStatistics_CompletesWithinAcceptableTime()
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await this.client.GetAsync(new Uri("/api/statistics/most-popular-products?count=10", UriKind.Relative));
        stopwatch.Stop();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(10000), "GetStatistics should complete within 10 seconds");
    }

    [Test]
    public async Task ConcurrentRequests_HandleMultipleClients()
    {
        var tasks = new List<Task<HttpResponseMessage>>();

        // Create 10 concurrent requests
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(this.client.GetAsync(new Uri("/api/customers", UriKind.Relative)));
        }

        var responses = await Task.WhenAll(tasks);

        // All requests should succeed
        foreach (var response in responses)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }

    [Test]
    public async Task CreateCustomer_CompletesWithinAcceptableTime()
    {
        var customer = new CustomerModel
        {
            Id = 0,
            Name = "Performance",
            Surname = "Test",
            BirthDate = new DateTime(1990, 1, 1),
            DiscountValue = 15,
            ReceiptsIds = new List<int>()
        };

        var stopwatch = Stopwatch.StartNew();
        var response = await this.client.PostAsJsonAsync("/api/customers", customer);
        stopwatch.Stop();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(3000), "CreateCustomer should complete within 3 seconds");
    }

    [Test]
    public async Task CreateProduct_CompletesWithinAcceptableTime()
    {
        var product = new ProductModel
        {
            Id = 0,
            ProductCategoryId = 1,
            ProductName = "Performance Test Product",
            Price = 100m,
            CategoryName = "Dairy products", // Use existing category name
            ReceiptDetailIds = new List<int>()
        };

        var stopwatch = Stopwatch.StartNew();
        var response = await this.client.PostAsJsonAsync("/api/products", product);
        stopwatch.Stop();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(5000), "CreateProduct should complete within 3 seconds");
    }

    [Test]
    public async Task UpdateCustomer_CompletesWithinAcceptableTime()
    {
        var customer = new CustomerModel
        {
            Id = 1,
            Name = "Updated Performance",
            Surname = "Test",
            BirthDate = new DateTime(1990, 1, 1),
            DiscountValue = 20,
            ReceiptsIds = new List<int> { 1 }
        };

        var stopwatch = Stopwatch.StartNew();
        var response = await this.client.PutAsJsonAsync("/api/customers/1", customer);
        stopwatch.Stop();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(3000), "UpdateCustomer should complete within 3 seconds");
    }

    [Test]
    public async Task DeleteCustomer_CompletesWithinAcceptableTime()
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await this.client.DeleteAsync(new Uri("/api/customers/2", UriKind.Relative));
        stopwatch.Stop();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(2000), "DeleteCustomer should complete within 2 seconds");
    }
}
