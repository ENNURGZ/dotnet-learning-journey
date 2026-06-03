using System.Net;
using System.Net.Http.Json;
using Business.Models;
using NUnit.Framework;

namespace WebApi.Tests;

[TestFixture]
public class ValidationTests
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
    public async Task CreateCustomer_ReturnsBadRequest_WhenNameIsNull()
    {
        var customer = new CustomerModel
        {
            Id = 0,
            Name = null!,
            Surname = "Test",
            BirthDate = new DateTime(1990, 1, 1),
            DiscountValue = 15,
            ReceiptsIds = new List<int>()
        };

        var response = await this.client.PostAsJsonAsync("/api/customers", customer);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task CreateCustomer_ReturnsBadRequest_WhenSurnameIsWhitespace()
    {
        var customer = new CustomerModel
        {
            Id = 0,
            Name = "Test",
            Surname = "   ",
            BirthDate = new DateTime(1990, 1, 1),
            DiscountValue = 15,
            ReceiptsIds = new List<int>()
        };

        var response = await this.client.PostAsJsonAsync("/api/customers", customer);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task CreateCustomer_ReturnsBadRequest_WhenBirthDateIsInFuture()
    {
        var customer = new CustomerModel
        {
            Id = 0,
            Name = "Test",
            Surname = "Customer",
            BirthDate = DateTime.Now.AddDays(1),
            DiscountValue = 15,
            ReceiptsIds = new List<int>()
        };

        var response = await this.client.PostAsJsonAsync("/api/customers", customer);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task CreateCustomer_ReturnsBadRequest_WhenBirthDateIsTooOld()
    {
        var customer = new CustomerModel
        {
            Id = 0,
            Name = "Test",
            Surname = "Customer",
            BirthDate = new DateTime(1800, 1, 1),
            DiscountValue = 15,
            ReceiptsIds = new List<int>()
        };

        var response = await this.client.PostAsJsonAsync("/api/customers", customer);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task CreateCustomer_ReturnsBadRequest_WhenDiscountValueIsNegative()
    {
        var customer = new CustomerModel
        {
            Id = 0,
            Name = "Test",
            Surname = "Customer",
            BirthDate = new DateTime(1990, 1, 1),
            DiscountValue = -5,
            ReceiptsIds = new List<int>()
        };

        var response = await this.client.PostAsJsonAsync("/api/customers", customer);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task CreateProduct_ReturnsBadRequest_WhenProductNameIsEmpty()
    {
        var product = new ProductModel
        {
            Id = 0,
            ProductCategoryId = 1,
            ProductName = "",
            Price = 100m,
            CategoryName = "Test Category"
        };

        var response = await this.client.PostAsJsonAsync("/api/products", product);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task CreateProduct_ReturnsBadRequest_WhenPriceIsNegative()
    {
        var product = new ProductModel
        {
            Id = 0,
            ProductCategoryId = 1,
            ProductName = "Test Product",
            Price = -50m,
            CategoryName = "Test Category"
        };

        var response = await this.client.PostAsJsonAsync("/api/products", product);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task CreateCategory_ReturnsBadRequest_WhenCategoryNameIsNull()
    {
        var category = new CategoryModel
        {
            Id = 0,
            Name = null!,
            ProductIds = new List<int>()
        };

        var response = await this.client.PostAsJsonAsync("/api/products/categories", category);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task CreateCategory_ReturnsBadRequest_WhenCategoryNameIsWhitespace()
    {
        var category = new CategoryModel
        {
            Id = 0,
            Name = "   ",
            ProductIds = new List<int>()
        };

        var response = await this.client.PostAsJsonAsync("/api/products/categories", category);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetStatistics_ReturnsBadRequest_WhenCountIsNegative()
    {
        var response = await this.client.GetAsync(new Uri("/api/statistics/most-popular-products?count=-1", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetStatistics_ReturnsBadRequest_WhenCustomerIdIsNegative()
    {
        var response = await this.client.GetAsync(new Uri("/api/statistics/customers-most-popular-products?count=5&customerId=-1", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetStatistics_ReturnsBadRequest_WhenStartDateIsAfterEndDate()
    {
        var startDate = new DateTime(2021, 12, 31);
        var endDate = new DateTime(2021, 1, 1);
        var response = await this.client.GetAsync(new Uri($"/api/statistics/most-valuable-customers?count=5&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}
