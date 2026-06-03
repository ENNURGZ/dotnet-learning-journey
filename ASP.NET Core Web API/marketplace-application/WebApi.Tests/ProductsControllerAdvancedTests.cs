using System.Net;
using System.Net.Http.Json;
using Business.Models;
using NUnit.Framework;

namespace WebApi.Tests;

[TestFixture]
public class ProductsControllerAdvancedTests
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
    public async Task GetByFilter_ReturnsOk_WhenValidFilter()
    {
        var filter = new FilterSearchModel
        {
            CategoryId = 1,
            MinPrice = 10,
            MaxPrice = 100
        };

        var response = await this.client.PostAsJsonAsync("/api/products/filter", filter);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>();
        Assert.That(products, Is.Not.Null);
    }

    [Test]
    public async Task GetByFilter_ReturnsOk_WhenNoFilter()
    {
        var filter = new FilterSearchModel();

        var response = await this.client.PostAsJsonAsync("/api/products/filter", filter);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>();
        Assert.That(products, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task GetAllProductCategories_ReturnsOk()
    {
        var response = await this.client.GetAsync(new Uri("/api/products/categories", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var categories = await response.Content.ReadFromJsonAsync<List<CategoryModel>>();
        Assert.That(categories, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task AddCategory_ReturnsCreated_WhenValidCategory()
    {
        var newCategory = new CategoryModel
        {
            Id = 0,
            Name = "Test Category",
            ProductIds = new List<int>()
        };

        var response = await this.client.PostAsJsonAsync("/api/products/categories", newCategory);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task AddCategory_ReturnsBadRequest_WhenInvalidCategory()
    {
        var invalidCategory = new CategoryModel
        {
            Id = 0,
            Name = "", // Invalid: empty name
            ProductIds = new List<int>()
        };

        var response = await this.client.PostAsJsonAsync("/api/products/categories", invalidCategory);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UpdateCategory_ReturnsNoContent_WhenValidCategory()
    {
        var updatedCategory = new CategoryModel
        {
            Id = 1,
            Name = "Updated Category",
            ProductIds = new List<int> { 1, 2 }
        };

        var response = await this.client.PutAsJsonAsync("/api/products/categories/1", updatedCategory);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task RemoveCategory_ReturnsNoContent_WhenCategoryExists()
    {
        var response = await this.client.DeleteAsync(new Uri("/api/products/categories/2", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
}
