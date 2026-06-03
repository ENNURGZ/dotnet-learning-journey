using AutoMapper;
using Business.Models;
using Data.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Business.Tests;

/// <summary>
/// Unit tests for AutomapperProfile class.
/// </summary>
public class AutomapperProfileTests
{
    private IMapper mapper = null!;

    [SetUp]
    public void SetUp()
    {
        using var logger = new NullLoggerFactory();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>(), logger);
        this.mapper = configuration.CreateMapper();
    }

    #region Product to ProductModel Mapping Tests

    [Test]
    public void ProductToProductModel_ValidProduct_ShouldMapCorrectly()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            ProductName = "Test Product",
            Price = 10.99m,
            ProductCategoryId = 2,
            Category = new ProductCategory { Id = 2, CategoryName = "Test Category" },
            ReceiptDetails = new List<ReceiptDetail>
            {
                new ReceiptDetail { Id = 1 },
                new ReceiptDetail { Id = 2 }
            }
        };

        // Act
        var result = this.mapper.Map<ProductModel>(product);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.ProductName, Is.EqualTo("Test Product"));
            Assert.That(result.Price, Is.EqualTo(10.99m));
            Assert.That(result.ProductCategoryId, Is.EqualTo(2));
            Assert.That(result.CategoryName, Is.EqualTo("Test Category"));
            Assert.That(result.ReceiptDetailIds, Is.EqualTo((int[])[1, 2]));
        });
    }

    [Test]
    public void ProductToProductModel_ProductWithNoReceiptDetails_ShouldMapWithEmptyReceiptDetailIds()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            ProductName = "Test Product",
            Price = 10.99m,
            ProductCategoryId = 2,
            Category = new ProductCategory { Id = 2, CategoryName = "Test Category" },
            ReceiptDetails = new List<ReceiptDetail>()
        };

        // Act
        var result = this.mapper.Map<ProductModel>(product);

        // Assert
        Assert.That(result.ReceiptDetailIds, Is.Empty);
    }

    #endregion

    #region ProductModel to Product Mapping Tests

    [Test]
    public void ProductModelToProduct_ValidProductModel_ShouldMapCorrectly()
    {
        // Arrange
        var productModel = new ProductModel
        {
            Id = 1,
            ProductName = "Test Product",
            Price = 10.99m,
            ProductCategoryId = 2,
            CategoryName = "Test Category",
            ReceiptDetailIds = new List<int> { 1, 2 }
        };

        // Act
        var result = this.mapper.Map<Product>(productModel);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.ProductName, Is.EqualTo("Test Product"));
            Assert.That(result.Price, Is.EqualTo(10.99m));
            Assert.That(result.ProductCategoryId, Is.EqualTo(2));
            Assert.That(result.Category, Is.Null);
            Assert.That(result.ReceiptDetails, Is.Null);
        });
    }

    #endregion

    #region Receipt to ReceiptModel Mapping Tests

    [Test]
    public void ReceiptToReceiptModel_ValidReceipt_ShouldMapCorrectly()
    {
        // Arrange
        var receipt = new Receipt
        {
            Id = 1,
            CustomerId = 2,
            OperationDate = new DateTime(2023, 1, 1),
            ReceiptDetails = new List<ReceiptDetail>
            {
                new ReceiptDetail { Id = 1 },
                new ReceiptDetail { Id = 2 }
            }
        };

        // Act
        var result = this.mapper.Map<ReceiptModel>(receipt);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.CustomerId, Is.EqualTo(2));
            Assert.That(result.OperationDate, Is.EqualTo(new DateTime(2023, 1, 1)));
            Assert.That(result.ReceiptDetailsIds, Is.EqualTo((int[])[1, 2]));
        });
    }

    #endregion

    #region ReceiptModel to Receipt Mapping Tests

    [Test]
    public void ReceiptModelToReceipt_ValidReceiptModel_ShouldMapCorrectly()
    {
        // Arrange
        var receiptModel = new ReceiptModel
        {
            Id = 1,
            CustomerId = 2,
            OperationDate = new DateTime(2023, 1, 1),
            ReceiptDetailsIds = new List<int> { 1, 2 }
        };

        // Act
        var result = this.mapper.Map<Receipt>(receiptModel);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.CustomerId, Is.EqualTo(2));
            Assert.That(result.OperationDate, Is.EqualTo(new DateTime(2023, 1, 1)));
            Assert.That(result.Customer, Is.Null);
            Assert.That(result.ReceiptDetails, Is.Null);
        });
    }

    #endregion

    #region ReceiptDetail Mapping Tests

    [Test]
    public void ReceiptDetailToReceiptDetailModel_ValidReceiptDetail_ShouldMapCorrectly()
    {
        // Arrange
        var receiptDetail = new ReceiptDetail
        {
            Id = 1,
            ReceiptId = 2,
            ProductId = 3,
            Quantity = 5,
            UnitPrice = 10.99m,
            DiscountUnitPrice = 9.99m
        };

        // Act
        var result = this.mapper.Map<ReceiptDetailModel>(receiptDetail);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.ReceiptId, Is.EqualTo(2));
            Assert.That(result.ProductId, Is.EqualTo(3));
            Assert.That(result.Quantity, Is.EqualTo(5));
            Assert.That(result.UnitPrice, Is.EqualTo(10.99m));
            Assert.That(result.DiscountUnitPrice, Is.EqualTo(9.99m));
        });
    }

    [Test]
    public void ReceiptDetailModelToReceiptDetail_ValidReceiptDetailModel_ShouldMapCorrectly()
    {
        // Arrange
        var receiptDetailModel = new ReceiptDetailModel
        {
            Id = 1,
            ReceiptId = 2,
            ProductId = 3,
            Quantity = 5,
            UnitPrice = 10.99m,
            DiscountUnitPrice = 9.99m
        };

        // Act
        var result = this.mapper.Map<ReceiptDetail>(receiptDetailModel);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.ReceiptId, Is.EqualTo(2));
            Assert.That(result.ProductId, Is.EqualTo(3));
            Assert.That(result.Quantity, Is.EqualTo(5));
            Assert.That(result.UnitPrice, Is.EqualTo(10.99m));
            Assert.That(result.DiscountUnitPrice, Is.EqualTo(9.99m));
        });
    }

    #endregion

    #region Customer to CustomerModel Mapping Tests

    [Test]
    public void CustomerToCustomerModel_ValidCustomer_ShouldMapCorrectly()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            PersonId = 2,
            DiscountValue = 10,
            Person = new Person
            {
                Id = 2,
                Name = "John",
                Surname = "Doe",
                BirthDate = new DateTime(1990, 1, 1)
            },
            Receipts = new List<Receipt>
            {
                new Receipt { Id = 1 },
                new Receipt { Id = 2 }
            }
        };

        // Act
        var result = this.mapper.Map<CustomerModel>(customer);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.DiscountValue, Is.EqualTo(10));
            Assert.That(result.Name, Is.EqualTo("John"));
            Assert.That(result.Surname, Is.EqualTo("Doe"));
            Assert.That(result.BirthDate, Is.EqualTo(new DateTime(1990, 1, 1)));
            Assert.That(result.ReceiptsIds, Is.EqualTo((int[])[1, 2]));
        });
    }

    #endregion

    #region CustomerModel to Customer Mapping Tests

    [Test]
    public void CustomerModelToCustomer_ValidCustomerModel_ShouldMapCorrectly()
    {
        // Arrange
        var customerModel = new CustomerModel
        {
            Id = 1,
            Name = "John",
            Surname = "Doe",
            BirthDate = new DateTime(1990, 1, 1),
            DiscountValue = 10,
            ReceiptsIds = new List<int> { 1, 2 }
        };

        // Act
        var result = this.mapper.Map<Customer>(customerModel);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.PersonId, Is.EqualTo(1));
            Assert.That(result.DiscountValue, Is.EqualTo(10));
            Assert.That(result.Person, Is.Not.Null);
            Assert.That(result.Person!.Id, Is.EqualTo(1));
            Assert.That(result.Person.Name, Is.EqualTo("John"));
            Assert.That(result.Person.Surname, Is.EqualTo("Doe"));
            Assert.That(result.Person.BirthDate, Is.EqualTo(new DateTime(1990, 1, 1)));
        });
    }

    #endregion

    #region ProductCategory to CategoryModel Mapping Tests

    [Test]
    public void ProductCategoryToCategoryModel_ValidProductCategory_ShouldMapCorrectly()
    {
        // Arrange
        var productCategory = new ProductCategory
        {
            Id = 1,
            CategoryName = "Test Category",
            Products = new List<Product>
            {
                new Product { Id = 1 },
                new Product { Id = 2 }
            }
        };

        // Act
        var result = this.mapper.Map<CategoryModel>(productCategory);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Test Category"));
            Assert.That(result.ProductIds, Is.EqualTo((int[])[1, 2]));
        });
    }

    #endregion

    #region CategoryModel to ProductCategory Mapping Tests

    [Test]
    public void CategoryModelToProductCategory_ValidCategoryModel_ShouldMapCorrectly()
    {
        // Arrange
        var categoryModel = new CategoryModel
        {
            Id = 1,
            Name = "Test Category",
            ProductIds = new List<int> { 1, 2 }
        };

        // Act
        var result = this.mapper.Map<ProductCategory>(categoryModel);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.CategoryName, Is.EqualTo("Test Category"));
        });
    }

    #endregion

    #region Configuration Tests

    [Test]
    public void AutomapperProfile_Configuration_ShouldBeValid()
    {
        // Arrange & Act
        using var logger = new NullLoggerFactory();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>(), logger);

        // Assert
        configuration.AssertConfigurationIsValid();
    }

    #endregion
}
