using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.Repository;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests;

[TestFixture]
public class CartControllerTests
{
    private Mock<IStoreRepository> mockRepository = null!;
    private CartController controller = null!;
    private Cart cart = null!;

    [SetUp]
    public void Setup()
    {
        this.mockRepository = new Mock<IStoreRepository>();
        this.cart = new Cart();
        this.controller = new CartController(this.mockRepository.Object, this.cart);
    }

    [TearDown]
    public void TearDown()
    {
        this.controller?.Dispose();
    }

    [Test]
    public void CartController_Index_GET_ReturnsViewWithEmptyCart()
    {
        // Act
        var result = this.controller.Index("/");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            var viewModel = viewResult!.Model as CartViewModel;
            Assert.That(viewModel!.Cart, Is.SameAs(this.cart));
            Assert.That(viewModel.Cart!.Lines, Is.Empty);
            Assert.That(viewModel.ReturnUrl, Is.EqualTo(new Uri("/", UriKind.Relative)));
        });
    }

    [Test]
    public void CartController_Index_GET_ReturnsViewWithExistingCart()
    {
        // Arrange
        this.cart.AddItem(new Product { ProductId = 1, Name = "Test Product", Price = 10.00m }, 2);

        // Act
        var result = this.controller.Index("/test");
        // Assert
        Assert.Multiple(() =>
        {
            var viewResult = result as ViewResult;
            var viewModel = viewResult!.Model as CartViewModel;

            Assert.That(viewModel?.Cart?.Lines, Has.Count.EqualTo(1));
            Assert.That(viewModel!.ReturnUrl, Is.EqualTo(new Uri("/test", UriKind.Relative)));
        });
    }

    [Test]
    public void CartController_Index_POST_WithValidProduct_AddsToCart()
    {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Test Product", Price = 10.00m };
        this.mockRepository.Setup(r => r.Products).Returns(new[] { product }.AsQueryable());

        // Act
        var result = this.controller.Index(1, "/test");

        // Assert
        Assert.Multiple(() =>
        {
            var viewResult = result as ViewResult;
            var viewModel = viewResult!.Model as CartViewModel;

            Assert.That(viewModel?.Cart?.Lines, Has.Count.EqualTo(1));
            Assert.That(this.cart.Lines[0].Product.ProductId, Is.EqualTo(1));
            Assert.That(viewModel!.ReturnUrl, Is.EqualTo(new Uri("/test", UriKind.Relative)));
        });
    }

    [Test]
    public void CartController_Index_POST_WithExistingProduct_IncreasesQuantity()
    {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Test Product", Price = 10.00m };
        this.mockRepository.Setup(r => r.Products).Returns(new[] { product }.AsQueryable());
        this.cart.AddItem(product, 2);

        // Act
        this.controller.Index(1, "/test");

        // Assert
        Assert.That(this.cart.Lines[0].Quantity, Is.EqualTo(3));
    }

    [Test]
    public void CartController_Index_POST_WithNonExistentProduct_RedirectsToHome()
    {
        // Arrange
        this.mockRepository.Setup(r => r.Products).Returns(Array.Empty<Product>().AsQueryable());

        // Act
        var result = this.controller.Index(999, "/test");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult!.ActionName, Is.EqualTo("Index"));
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
        });
    }

    [Test]
    public void CartController_Remove_RemovesSpecifiedProduct()
    {
        // Arrange
        var p1 = new Product { ProductId = 1, Name = "P1" };
        var p2 = new Product { ProductId = 2, Name = "P2" };
        this.cart.AddItem(p1, 1);
        this.cart.AddItem(p2, 1);

        // Act
        this.controller.Remove(1, "/test");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(this.cart.Lines, Has.Count.EqualTo(1));
            Assert.That(this.cart.Lines[0].Product.ProductId, Is.EqualTo(2));
        });
    }
}
