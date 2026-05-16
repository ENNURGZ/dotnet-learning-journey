using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.Repository;

namespace SportsStore.Tests;

[Category("Step5")]
public class AdminControllerTests
{
    [Test]
    public void Orders_ReturnsViewWithOrders()
    {
        // Arrange
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockStoreRepo = new Mock<IStoreRepository>();
        var orders = new List<Order>
        {
            new Order { OrderId = 1, Name = "Test Order 1", Shipped = false },
            new Order { OrderId = 2, Name = "Test Order 2", Shipped = true }
        }.AsQueryable();

        mockOrderRepo.Setup(m => m.Orders).Returns(orders);

        var controller = new AdminController(mockStoreRepo.Object, mockOrderRepo.Object);

        // Act
        var result = controller.Orders();

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        var viewResult = result as ViewResult;
        Assert.That(viewResult.Model, Is.EqualTo(orders));
    }

    [Test]
    public void Products_ReturnsViewWithProducts()
    {
        // Arrange
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockStoreRepo = new Mock<IStoreRepository>();
        var products = new List<Product>
        {
            new Product { ProductId = 1, Name = "Test Product 1", Price = 10.00m },
            new Product { ProductId = 2, Name = "Test Product 2", Price = 20.00m }
        }.AsQueryable();

        mockStoreRepo.Setup(m => m.Products).Returns(products);

        var controller = new AdminController(mockStoreRepo.Object, mockOrderRepo.Object);

        // Act
        var result = controller.Products();

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        var viewResult = result as ViewResult;
        Assert.That(viewResult.Model, Is.EqualTo(products));
    }

    [Test]
    public void MarkShipped_WithValidOrderId_MarksOrderAsShipped()
    {
        // Arrange
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockStoreRepo = new Mock<IStoreRepository>();
        var order = new Order { OrderId = 1, Name = "Test Order", Shipped = false };
        var orders = new List<Order> { order }.AsQueryable();

        mockOrderRepo.Setup(m => m.Orders).Returns(orders);

        var controller = new AdminController(mockStoreRepo.Object, mockOrderRepo.Object);

        // Act
        var result = controller.MarkShipped(1);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(order.Shipped, Is.True);
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult!.ActionName, Is.EqualTo("Orders"));
        });
        mockOrderRepo.Verify(m => m.SaveOrder(order), Times.Once);
    }

    [Test]
    public void MarkShipped_WithInvalidOrderId_DoesNotMarkOrder()
    {
        // Arrange
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockStoreRepo = new Mock<IStoreRepository>();
        var orders = new List<Order>().AsQueryable();

        mockOrderRepo.Setup(m => m.Orders).Returns(orders);

        var controller = new AdminController(mockStoreRepo.Object, mockOrderRepo.Object);

        // Act
        var result = controller.MarkShipped(999);

        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        mockOrderRepo.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
    }

    [Test]
    public void Reset_WithValidOrderId_UnmarksOrderAsShipped()
    {
        // Arrange
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockStoreRepo = new Mock<IStoreRepository>();
        var order = new Order { OrderId = 1, Name = "Test Order", Shipped = true };
        var orders = new List<Order> { order }.AsQueryable();

        mockOrderRepo.Setup(m => m.Orders).Returns(orders);

        var controller = new AdminController(mockStoreRepo.Object, mockOrderRepo.Object);

        // Act
        var result = controller.Reset(1);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(order.Shipped, Is.False);
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult!.ActionName, Is.EqualTo("Orders"));
        });
        mockOrderRepo.Verify(m => m.SaveOrder(order), Times.Once);
    }

    [Test]
    public void Details_WithValidProductId_ReturnsViewWithProduct()
    {
        // Arrange
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockStoreRepo = new Mock<IStoreRepository>();
        var product = new Product { ProductId = 1, Name = "Test Product", Price = 10.00m };
        var products = new List<Product> { product }.AsQueryable();

        mockStoreRepo.Setup(m => m.Products).Returns(products);

        var controller = new AdminController(mockStoreRepo.Object, mockOrderRepo.Object);

        // Act
        var result = controller.Details(1);

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        var viewResult = result as ViewResult;
        Assert.That(viewResult.Model, Is.EqualTo(product));
    }

    [Test]
    public void Edit_WithValidProductId_ReturnsViewWithProduct()
    {
        // Arrange
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockStoreRepo = new Mock<IStoreRepository>();
        var product = new Product { ProductId = 1, Name = "Test Product", Price = 10.00m };
        var products = new List<Product> { product }.AsQueryable();

        mockStoreRepo.Setup(m => m.Products).Returns(products);

        var controller = new AdminController(mockStoreRepo.Object, mockOrderRepo.Object);

        // Act
        var result = controller.Edit(1);

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        var viewResult = result as ViewResult;
        Assert.That(viewResult.Model, Is.EqualTo(product));
    }

    [Test]
    public void Edit_WithValidModel_SavesProductAndRedirects()
    {
        // Arrange
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockStoreRepo = new Mock<IStoreRepository>();
        var product = new Product { ProductId = 1, Name = "Updated Product", Price = 15.00m };

        var controller = new AdminController(mockStoreRepo.Object, mockOrderRepo.Object);

        // Act
        var result = controller.Edit(product);

        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        var redirectResult = result as RedirectToActionResult;
        Assert.That(redirectResult!.ActionName, Is.EqualTo("Products"));
        mockStoreRepo.Verify(m => m.SaveProduct(product), Times.Once);
    }

    [Test]
    public void Edit_WithInvalidModel_ReturnsViewWithProduct()
    {
        // Arrange
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockStoreRepo = new Mock<IStoreRepository>();
        var product = new Product { ProductId = 1, Name = "", Price = -1.00m };

        var controller = new AdminController(mockStoreRepo.Object, mockOrderRepo.Object);
        controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = controller.Edit(product);

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        var viewResult = result as ViewResult;
        Assert.That(viewResult!.Model, Is.EqualTo(product));
        mockStoreRepo.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
    }

    [Test]
    public void Create_ReturnsViewWithNewProduct()
    {
        // Arrange
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockStoreRepo = new Mock<IStoreRepository>();

        var controller = new AdminController(mockStoreRepo.Object, mockOrderRepo.Object);

        // Act
        var result = controller.Create();

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        var viewResult = result as ViewResult;
        Assert.That(viewResult.Model, Is.InstanceOf<Product>());
    }

    [Test]
    public void Create_WithValidModel_SavesProductAndRedirects()
    {
        // Arrange
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockStoreRepo = new Mock<IStoreRepository>();
        var product = new Product { Name = "New Product", Price = 10.00m, Category = "Test" };

        var controller = new AdminController(mockStoreRepo.Object, mockOrderRepo.Object);

        // Act
        var result = controller.Create(product);

        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        var redirectResult = result as RedirectToActionResult;
        Assert.That(redirectResult!.ActionName, Is.EqualTo("Products"));
        mockStoreRepo.Verify(m => m.SaveProduct(product), Times.Once);
    }

    [Test]
    public void Delete_WithValidProductId_ReturnsViewWithProduct()
    {
        // Arrange
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockStoreRepo = new Mock<IStoreRepository>();
        var product = new Product { ProductId = 1, Name = "Test Product", Price = 10.00m };
        var products = new List<Product> { product }.AsQueryable();

        mockStoreRepo.Setup(m => m.Products).Returns(products);

        var controller = new AdminController(mockStoreRepo.Object, mockOrderRepo.Object);

        // Act
        var result = controller.Delete(1);

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        var viewResult = result as ViewResult;
        Assert.That(viewResult!.Model, Is.EqualTo(product));
    }

    [Test]
    public void DeleteProduct_WithValidProductId_DeletesProductAndRedirects()
    {
        // Arrange
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockStoreRepo = new Mock<IStoreRepository>();
        var product = new Product { ProductId = 1, Name = "Test Product", Price = 10.00m };
        var products = new List<Product> { product }.AsQueryable();

        mockStoreRepo.Setup(m => m.Products).Returns(products);

        var controller = new AdminController(mockStoreRepo.Object, mockOrderRepo.Object);

        // Act
        var result = controller.DeleteProduct(1);

        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        var redirectResult = result as RedirectToActionResult;
        Assert.That(redirectResult!.ActionName, Is.EqualTo("Products"));
        mockStoreRepo.Verify(m => m.DeleteProduct(product), Times.Once);
    }

    [Test]
    public void DeleteProduct_WithInvalidProductId_DoesNotDeleteProduct()
    {
        // Arrange
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockStoreRepo = new Mock<IStoreRepository>();
        var products = new List<Product>().AsQueryable();

        mockStoreRepo.Setup(m => m.Products).Returns(products);

        var controller = new AdminController(mockStoreRepo.Object, mockOrderRepo.Object);

        // Act
        var result = controller.DeleteProduct(999);

        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        mockStoreRepo.Verify(m => m.DeleteProduct(It.IsAny<Product>()), Times.Never);
    }
}
