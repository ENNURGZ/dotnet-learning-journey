using System.Net;
using System.Net.Http.Json;
using Business.Models;
using NUnit.Framework;

namespace WebApi.Tests;

[TestFixture]
public class ProductsControllerIntegrationTests
{
    private CustomWebApplicationFactory factory = null!;
    private HttpClient client = null!;

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
    public async Task GetAll_ReturnsOkAndProducts()
    {
        var response = await this.client.GetAsync(new Uri("/api/products", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>();
        Assert.That(products, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task GetById_ReturnsOk_WhenProductExists()
    {
        var response = await this.client.GetAsync(new Uri("/api/products/1", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var product = await response.Content.ReadFromJsonAsync<ProductModel>();
        Assert.That(product, Is.Not.Null);
        Assert.That(product!.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var response = await this.client.GetAsync(new Uri("/api/products/9999", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Create_ReturnsCreated_WhenValidProduct()
    {
        var newProduct = new ProductModel
        {
            Id = 0,
            ProductCategoryId = 1,
            ProductName = "Test Product",
            Price = 100.50m,
            CategoryName = "Dairy products", // Use existing category name from test data
            ReceiptDetailIds = new List<int>() // Initialize empty collection
        };

        var response = await this.client.PostAsJsonAsync("/api/products", newProduct);

        // Debug: Print response details if it fails
        if (response.StatusCode != HttpStatusCode.Created)
        {
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Status: {response.StatusCode}");
            Console.WriteLine($"Response Content: {content}");
        }

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task Create_ReturnsBadRequest_WhenInvalidProduct()
    {
        var invalidProduct = new ProductModel
        {
            Id = 0,
            ProductCategoryId = 1,
            ProductName = "", // Invalid: empty name
            Price = -10m, // Invalid: negative price
            CategoryName = "Test Category",
            ReceiptDetailIds = new List<int>()
        };

        var response = await this.client.PostAsJsonAsync("/api/products", invalidProduct);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Update_ReturnsNoContent_WhenValidProduct()
    {
        var updatedProduct = new ProductModel
        {
            Id = 1,
            ProductCategoryId = 1,
            ProductName = "Updated Product",
            Price = 150.75m,
            CategoryName = "Dairy products", // Use existing category name
            ReceiptDetailIds = new List<int>()
        };

        var response = await this.client.PutAsJsonAsync("/api/products/1", updatedProduct);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var product = new ProductModel
        {
            Id = 1,
            ProductCategoryId = 1,
            ProductName = "Test Product",
            Price = 100m,
            CategoryName = "Test Category",
            ReceiptDetailIds = new List<int>()
        };

        var response = await this.client.PutAsJsonAsync("/api/products/2", product);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Delete_ReturnsNoContent_WhenProductExists()
    {
        var response = await this.client.DeleteAsync(new Uri("/api/products/2", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task Delete_ReturnsNoContent_WhenProductDoesNotExist()
    {
        var response = await this.client.DeleteAsync(new Uri("/api/products/9999", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
}
