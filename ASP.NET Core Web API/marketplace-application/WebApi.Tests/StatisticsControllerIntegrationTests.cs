using System.Net;
using System.Net.Http.Json;
using Business.Models;
using NUnit.Framework;

namespace WebApi.Tests;

[TestFixture]
public class StatisticsControllerIntegrationTests
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
    public async Task GetMostPopularProducts_ReturnsOk_WhenValidCount()
    {
        var response = await this.client.GetAsync(new Uri("/api/statistics/most-popular-products?count=5", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>();
        Assert.That(products, Is.Not.Null);
    }

    [Test]
    public async Task GetMostPopularProducts_ReturnsOk_WhenCountIsZero()
    {
        var response = await this.client.GetAsync(new Uri("/api/statistics/most-popular-products?count=0", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>();
        Assert.That(products, Is.Not.Null.And.Empty);
    }

    [Test]
    public async Task GetCustomersMostPopularProducts_ReturnsOk_WhenValidParameters()
    {
        var response = await this.client.GetAsync(new Uri("/api/statistics/customers-most-popular-products?count=3&customerId=1", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>();
        Assert.That(products, Is.Not.Null);
    }

    [Test]
    public async Task GetCustomersMostPopularProducts_ReturnsOk_WhenCustomerDoesNotExist()
    {
        var response = await this.client.GetAsync(new Uri("/api/statistics/customers-most-popular-products?count=3&customerId=9999", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>();
        Assert.That(products, Is.Not.Null.And.Empty);
    }

    [Test]
    public async Task GetMostValuableCustomers_ReturnsOk_WhenValidParameters()
    {
        var startDate = new DateTime(2021, 1, 1);
        var endDate = new DateTime(2021, 12, 31);
        var response = await this.client.GetAsync(new Uri($"/api/statistics/most-valuable-customers?count=5&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var customers = await response.Content.ReadFromJsonAsync<List<CustomerActivityModel>>();
        Assert.That(customers, Is.Not.Null);
    }

    [Test]
    public async Task GetMostValuableCustomers_ReturnsOk_WhenNoDataInPeriod()
    {
        var startDate = new DateTime(2020, 1, 1);
        var endDate = new DateTime(2020, 12, 31);
        var response = await this.client.GetAsync(new Uri($"/api/statistics/most-valuable-customers?count=5&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var customers = await response.Content.ReadFromJsonAsync<List<CustomerActivityModel>>();
        Assert.That(customers, Is.Not.Null.And.Empty);
    }

    [Test]
    public async Task GetIncomeOfCategory_ReturnsOk_WhenValidParameters()
    {
        var startDate = new DateTime(2021, 1, 1);
        var endDate = new DateTime(2021, 12, 31);
        var response = await this.client.GetAsync(new Uri($"/api/statistics/income-of-category?categoryId=1&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var income = await response.Content.ReadFromJsonAsync<decimal>();
        Assert.That(income, Is.Not.Null);
        Assert.That(income, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public async Task GetIncomeOfCategory_ReturnsOk_WhenCategoryDoesNotExist()
    {
        var startDate = new DateTime(2021, 1, 1);
        var endDate = new DateTime(2021, 12, 31);
        var response = await this.client.GetAsync(new Uri($"/api/statistics/income-of-category?categoryId=9999&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var income = await response.Content.ReadFromJsonAsync<decimal>();
        Assert.That(income, Is.EqualTo(0));
    }

    [Test]
    public async Task GetIncomeOfCategory_ReturnsOk_WhenNoDataInPeriod()
    {
        var startDate = new DateTime(2020, 1, 1);
        var endDate = new DateTime(2020, 12, 31);
        var response = await this.client.GetAsync(new Uri($"/api/statistics/income-of-category?categoryId=1&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var income = await response.Content.ReadFromJsonAsync<decimal>();
        Assert.That(income, Is.EqualTo(0));
    }
}
