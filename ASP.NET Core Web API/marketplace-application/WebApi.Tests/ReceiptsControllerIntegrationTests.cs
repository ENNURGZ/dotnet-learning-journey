using System.Net;
using System.Net.Http.Json;
using Business.Models;
using NUnit.Framework;

namespace WebApi.Tests;

[TestFixture]
public class ReceiptsControllerIntegrationTests
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
    public async Task GetAll_ReturnsOkAndReceipts()
    {
        var response = await this.client.GetAsync(new Uri("/api/receipts", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var receipts = await response.Content.ReadFromJsonAsync<List<ReceiptModel>>();
        Assert.That(receipts, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task GetById_ReturnsOk_WhenReceiptExists()
    {
        var response = await this.client.GetAsync(new Uri("/api/receipts/1", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var receipt = await response.Content.ReadFromJsonAsync<ReceiptModel>();
        Assert.That(receipt, Is.Not.Null);
        Assert.That(receipt!.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task GetById_ReturnsNotFound_WhenReceiptDoesNotExist()
    {
        var response = await this.client.GetAsync(new Uri("/api/receipts/9999", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Create_ReturnsCreated_WhenValidReceipt()
    {
        var newReceipt = new ReceiptModel
        {
            Id = 0,
            CustomerId = 1,
            OperationDate = DateTime.Now,
            IsCheckedOut = false,
            ReceiptDetailsIds = new List<int>()
        };

        var response = await this.client.PostAsJsonAsync("/api/receipts", newReceipt);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task Update_ReturnsNoContent_WhenValidReceipt()
    {
        var updatedReceipt = new ReceiptModel
        {
            Id = 1,
            CustomerId = 1,
            OperationDate = DateTime.Now,
            IsCheckedOut = true,
            ReceiptDetailsIds = new List<int> { 1, 2 }
        };

        var response = await this.client.PutAsJsonAsync("/api/receipts/1", updatedReceipt);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var receipt = new ReceiptModel
        {
            Id = 1,
            CustomerId = 1,
            OperationDate = DateTime.Now,
            IsCheckedOut = false,
            ReceiptDetailsIds = new List<int>()
        };

        var response = await this.client.PutAsJsonAsync("/api/receipts/2", receipt);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Delete_ReturnsNoContent_WhenReceiptExists()
    {
        var response = await this.client.DeleteAsync(new Uri("/api/receipts/3", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task Delete_ReturnsNoContent_WhenReceiptDoesNotExist()
    {
        var response = await this.client.DeleteAsync(new Uri("/api/receipts/9999", UriKind.Relative));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

}
