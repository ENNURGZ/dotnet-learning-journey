using System.Net;
using System.Net.Http.Json;
using Business.Models;
using NUnit.Framework;

namespace WebApi.Tests;

[TestFixture]
public class CustomersControllerIntegrationTests
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
    public async Task GetAll_ReturnsOkAndCustomers()
    {
        var response = await this.client.GetAsync(new Uri("/api/customers", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var customers = await response.Content.ReadFromJsonAsync<List<CustomerModel>>();
        Assert.That(customers, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task GetById_ReturnsOk_WhenCustomerExists()
    {
        var response = await this.client.GetAsync(new Uri("/api/customers/1", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var customer = await response.Content.ReadFromJsonAsync<CustomerModel>();
        Assert.That(customer, Is.Not.Null);
        Assert.That(customer.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task GetById_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        var response = await this.client.GetAsync(new Uri("/api/customers/9999", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Create_ReturnsCreated_WhenValidCustomer()
    {
        var newCustomer = new CustomerModel
        {
            Id = 0,
            Name = "Test",
            Surname = "Customer",
            BirthDate = new DateTime(1990, 1, 1),
            DiscountValue = 15,
            ReceiptsIds = new List<int>()
        };

        var response = await this.client.PostAsJsonAsync("/api/customers", newCustomer);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task Create_ReturnsBadRequest_WhenInvalidCustomer()
    {
        var invalidCustomer = new CustomerModel
        {
            Id = 0,
            Name = "", // Invalid: empty name
            Surname = "", // Invalid: empty surname
            BirthDate = DateTime.Now.AddDays(1), // Invalid: future birthdate.
            DiscountValue = -10, // Invalid: negative discount
            ReceiptsIds = new List<int>()
        };

        var response = await this.client.PostAsJsonAsync("/api/customers", invalidCustomer);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Update_ReturnsNoContent_WhenValidCustomer()
    {
        var updatedCustomer = new CustomerModel
        {
            Id = 1,
            Name = "Updated",
            Surname = "Customer",
            BirthDate = new DateTime(1985, 5, 15),
            DiscountValue = 25,
            ReceiptsIds = new List<int> { 1, 2 }
        };

        var response = await this.client.PutAsJsonAsync("/api/customers/1", updatedCustomer);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var customer = new CustomerModel
        {
            Id = 1,
            Name = "Test",
            Surname = "Customer",
            BirthDate = new DateTime(1990, 1, 1),
            DiscountValue = 15,
            ReceiptsIds = new List<int>()
        };

        var response = await this.client.PutAsJsonAsync("/api/customers/2", customer);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenInvalidCustomer()
    {
        var invalidCustomer = new CustomerModel
        {
            Id = 1,
            Name = "", // Invalid: empty name
            Surname = "Customer",
            BirthDate = new DateTime(1990, 1, 1),
            DiscountValue = 15,
            ReceiptsIds = new List<int>()
        };

        var response = await this.client.PutAsJsonAsync("/api/customers/1", invalidCustomer);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Delete_ReturnsNoContent_WhenCustomerExists()
    {
        var response = await this.client.DeleteAsync(new Uri("/api/customers/2", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task Delete_ReturnsNoContent_WhenCustomerDoesNotExist()
    {
        var response = await this.client.DeleteAsync(new Uri("/api/customers/9999", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task GetCustomersByProductId_ReturnsOk_WhenProductExists()
    {
        var response = await this.client.GetAsync(new Uri("/api/customers/by-product/1", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var customers = await response.Content.ReadFromJsonAsync<List<CustomerModel>>();
        Assert.That(customers, Is.Not.Null);
    }

    [Test]
    public async Task GetCustomersByProductId_ReturnsOk_WhenProductDoesNotExist()
    {
        var response = await this.client.GetAsync(new Uri("/api/customers/by-product/9999", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var customers = await response.Content.ReadFromJsonAsync<List<CustomerModel>>();
        Assert.That(customers, Is.Not.Null.And.Empty);
    }
}
