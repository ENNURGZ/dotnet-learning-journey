using System.Net;
using System.Net.Http.Json;
using Business.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace WebApi.Tests;

[TestFixture]
public class ReceiptsControllerAdvancedTests
{
    private WebApplicationFactory<Program> factory = null!;
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
    public async Task AddProduct_ReturnsOk_WhenValidProduct()
    {
        // Get existing receipts and products
        var allReceiptsResponse = await this.client.GetAsync(new Uri("/api/receipts", UriKind.Relative));
        var allReceipts = await allReceiptsResponse.Content.ReadFromJsonAsync<List<ReceiptModel>>();

        var allProductsResponse = await this.client.GetAsync(new Uri("/api/products", UriKind.Relative));
        var allProducts = await allProductsResponse.Content.ReadFromJsonAsync<List<ProductModel>>();

        var receiptId = allReceipts?.FirstOrDefault()?.Id ?? 1;
        var productId = allProducts?.FirstOrDefault()?.Id ?? 1;

        var response = await this.client.PostAsync($"/api/receipts/{receiptId}/products/{productId}?quantity=2", null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task AddProduct_ReturnsBadRequest_WhenReceiptDoesNotExist()
    {
        var response = await this.client.PostAsync("/api/receipts/9999/products/1?quantity=2", null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task AddProduct_ReturnsBadRequest_WhenProductDoesNotExist()
    {
        var response = await this.client.PostAsync("/api/receipts/1/products/9999?quantity=2", null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task RemoveProduct_ReturnsOk_WhenValidProduct()
    {
        // Get existing receipts and products
        var allReceiptsResponse = await this.client.GetAsync(new Uri("/api/receipts", UriKind.Relative));
        var allReceipts = await allReceiptsResponse.Content.ReadFromJsonAsync<List<ReceiptModel>>();

        var allProductsResponse = await this.client.GetAsync(new Uri("/api/products", UriKind.Relative));
        var allProducts = await allProductsResponse.Content.ReadFromJsonAsync<List<ProductModel>>();

        var receiptId = allReceipts?.FirstOrDefault()?.Id ?? 1;
        var productId = allProducts?.FirstOrDefault()?.Id ?? 1;

        var response = await this.client.DeleteAsync($"/api/receipts/{receiptId}/products/{productId}?quantity=1");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task RemoveProduct_ReturnsOk_WhenProductDoesNotExist()
    {
        // Get existing receipts
        var allReceiptsResponse = await this.client.GetAsync(new Uri("/api/receipts", UriKind.Relative));
        var allReceipts = await allReceiptsResponse.Content.ReadFromJsonAsync<List<ReceiptModel>>();

        var receiptId = allReceipts?.FirstOrDefault()?.Id ?? 1;

        var response = await this.client.DeleteAsync($"/api/receipts/{receiptId}/products/9999?quantity=1");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetReceiptDetails_ReturnsOk_WhenReceiptExists()
    {
        // First, get all receipts to see what IDs exist
        var allReceiptsResponse = await this.client.GetAsync(new Uri("/api/receipts", UriKind.Relative));
        var allReceipts = await allReceiptsResponse.Content.ReadFromJsonAsync<List<ReceiptModel>>();

        // Use the first available receipt ID
        var receiptId = allReceipts?.FirstOrDefault()?.Id ?? 1;

        var response = await this.client.GetAsync(new Uri($"/api/receipts/{receiptId}/details", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var details = await response.Content.ReadFromJsonAsync<List<ReceiptDetailModel>>();
        Assert.That(details, Is.Not.Null);
    }

    [Test]
    public async Task GetReceiptDetails_ReturnsBadRequest_WhenReceiptDoesNotExist()
    {
        var response = await this.client.GetAsync(new Uri("/api/receipts/9999/details", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task ToPay_ReturnsOk_WhenReceiptExists()
    {
        // First, get all receipts to see what IDs exist
        var allReceiptsResponse = await this.client.GetAsync(new Uri("/api/receipts", UriKind.Relative));
        var allReceipts = await allReceiptsResponse.Content.ReadFromJsonAsync<List<ReceiptModel>>();

        // Use the first available receipt ID
        var receiptId = allReceipts?.FirstOrDefault()?.Id ?? 1;

        var response = await this.client.GetAsync(new Uri($"/api/receipts/{receiptId}/total", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var total = await response.Content.ReadFromJsonAsync<decimal>();
        Assert.That(total, Is.Not.Null);
        Assert.That(total, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public async Task ToPay_ReturnsBadRequest_WhenReceiptDoesNotExist()
    {
        var response = await this.client.GetAsync(new Uri("/api/receipts/9999/total", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task CheckOut_ReturnsOk_WhenReceiptExists()
    {
        // Get existing customers first
        var allCustomersResponse = await this.client.GetAsync(new Uri("/api/customers", UriKind.Relative));
        var allCustomers = await allCustomersResponse.Content.ReadFromJsonAsync<List<CustomerModel>>();

        var customerId = allCustomers?.FirstOrDefault()?.Id ?? 1;

        // Create a new receipt first
        var newReceipt = new ReceiptModel
        {
            Id = 0,
            CustomerId = customerId,
            OperationDate = DateTime.Now,
            IsCheckedOut = false,
            ReceiptDetailsIds = new List<int>()
        };

            var createResponse = await this.client.PostAsJsonAsync("/api/receipts", newReceipt);
            Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            // Get the created receipt ID from the response
            var createdReceipt = await createResponse.Content.ReadFromJsonAsync<ReceiptModel>();
            Assert.That(createdReceipt, Is.Not.Null);

            // Now try to check out the created receipt
             var response = await this.client.PostAsync($"/api/receipts/{createdReceipt!.Id}/checkout", null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task CheckOut_ReturnsBadRequest_WhenReceiptDoesNotExist()
    {
        var response = await this.client.PostAsync("/api/receipts/9999/checkout", null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetReceiptsByPeriod_ReturnsOk_WhenValidPeriod()
    {
        var startDate = new DateTime(2021, 1, 1);
        var endDate = new DateTime(2021, 12, 31);
        var response = await this.client.GetAsync($"/api/receipts/period?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var receipts = await response.Content.ReadFromJsonAsync<List<ReceiptModel>>();
        Assert.That(receipts, Is.Not.Null);
    }

    [Test]
    public async Task GetReceiptsByPeriod_ReturnsOk_WhenNoDataInPeriod()
    {
        var startDate = new DateTime(2020, 1, 1);
        var endDate = new DateTime(2020, 12, 31);
        var response = await this.client.GetAsync($"/api/receipts/period?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var receipts = await response.Content.ReadFromJsonAsync<List<ReceiptModel>>();
        Assert.That(receipts, Is.Not.Null.And.Empty);
    }
}
