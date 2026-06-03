using Business.Models;
using Business.Services;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using FluentAssertions;
using Moq;

namespace Business.Tests;

public class ReceiptServiceTests
{
    [Test]
    public async Task GetAllAsync_Always_ReturnsAllReceipts()
    {
        //arrange
        var expected = GetTestReceiptsModels;
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        _ = mockUnitOfWork
            .Setup(x => x.ReceiptRepository.GetAllWithDetailsAsync())
            .ReturnsAsync(GetTestReceiptsEntities.AsEnumerable());

        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        var actual = await receiptService.GetAllAsync();

        //assert
        _ = actual.Should().BeEquivalentTo(expected);
    }

    [TestCase(5)]
    public async Task GetByIdAsync_ExistingId_ReturnsReceiptModel(int id)
    {
        //arrange
        var expected = GetTestReceiptsModels.FirstOrDefault(x => x.Id == id);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork
            .Setup(x => x.ReceiptRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(GetTestReceiptsEntities.FirstOrDefault(x => x.Id == id));

        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        var actual = await receiptService.GetByIdAsync(id);

        //assert
        _ = actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AddAsync_ValidModel_AddsReceipt()
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.ReceiptRepository.AddAsync(It.IsAny<Receipt>()));

        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var receipt = GetTestReceiptsModels.Last();

        //act
        await receiptService.AddAsync(receipt);

        //assert
        mockUnitOfWork.Verify(x => x.ReceiptRepository.AddAsync(It.Is<Receipt>(c => c.Id == receipt.Id
            && c.CustomerId == receipt.CustomerId && c.OperationDate == receipt.OperationDate)), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_ValidModel_UpdatesReceipt()
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.ReceiptRepository.Update(It.IsAny<Receipt>()));

        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var receipt = GetTestReceiptsModels.First();

        //act
        await receiptService.UpdateAsync(receipt);

        //assert
        mockUnitOfWork.Verify(x => x.ReceiptRepository.Update(It.Is<Receipt>(r => r.Id == receipt.Id &&
            r.IsCheckedOut == receipt.IsCheckedOut &&
            r.CustomerId == receipt.CustomerId && r.OperationDate == receipt.OperationDate)), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task DeleteAsync_ExistingId_DeletesReceiptWithDetails(int receiptId)
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var receipt = GetTestReceiptsEntities.First(x => x.Id == receiptId);
        var expectedDetailsLength = receipt.ReceiptDetails?.Count ?? 0;

        _ = mockUnitOfWork.Setup(m => m.ReceiptRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(receipt);
        _ = mockUnitOfWork.Setup(m => m.ReceiptRepository.DeleteByIdAsync(It.IsAny<int>()));
        _ = mockUnitOfWork.Setup(m => m.ReceiptDetailRepository.Delete(It.IsAny<ReceiptDetail>()));

        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        await receiptService.DeleteAsync(receiptId);

        //assert
        mockUnitOfWork.Verify(x => x.ReceiptRepository.DeleteByIdAsync(It.Is<int>(x => x == receiptId)), Times.Once());
        mockUnitOfWork.Verify(
            x => x.ReceiptDetailRepository.Delete(It.Is<ReceiptDetail>(detail => detail.ReceiptId == receiptId)),
            failMessage: "All existing receipt details must be deleted", times: Times.Exactly(expectedDetailsLength));
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once());
    }


    [TestCase(4)]
    [TestCase(5)]
    public async Task GetReceiptDetailsAsync_ExistingId_ReturnsDetailsByReceiptId(int receiptId)
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(x => x.ReceiptRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(GetTestReceiptsEntities.FirstOrDefault(x => x.Id == receiptId));
        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        var actual = await receiptService.GetReceiptDetailsAsync(receiptId);

        //assert
        var expected = GetTestReceiptsEntities.FirstOrDefault(x => x.Id == receiptId)?.ReceiptDetails;

        _ = actual.Should()
            .BeEquivalentTo(expected, options => options.Excluding(x => x.Product).Excluding(x => x.Receipt));
    }


    private static readonly int[] Period1ExpectedReceiptIds = [1, 2];
    private static readonly int[] Period2ExpectedReceiptIds = [3, 4, 5];

    private static IEnumerable<object[]> GetReceiptsByPeriodAsync_TestCases =>
    [
        [new DateTime(1965, 1, 1), new DateTime(1965, 2, 1), Period1ExpectedReceiptIds],
        [new DateTime(1965, 2, 1), new DateTime(1965, 4, 1), Period2ExpectedReceiptIds],
    ];

    [TestCaseSource(nameof(GetReceiptsByPeriodAsync_TestCases))]
    public async Task GetReceiptsByPeriodAsync_ValidPeriod_ReturnsReceiptsInPeriod(DateTime startDate, DateTime endDate,
        IEnumerable<int> expectedReceiptIds)
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(x => x.ReceiptRepository.GetAllWithDetailsAsync())
            .ReturnsAsync(GetTestReceiptsEntities.AsEnumerable());
        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        var actual = await receiptService.GetReceiptsByPeriodAsync(startDate, endDate);

        //assert
        var expected = GetTestReceiptsModels.Where(x => expectedReceiptIds.Contains(x.Id));

        _ = actual.Should().BeEquivalentTo(expected);
    }

    [TestCase(1, 99)]
    [TestCase(4, 546)]
    [TestCase(5, 1074)]
    public async Task ToPayAsync_ExistingId_ReturnsSumByReceiptIdWithDiscount(int receiptId, decimal expectedSum)
    {
        //arrange
        var receipt = GetTestReceiptsEntities.FirstOrDefault(x => x.Id == receiptId);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(x => x.ReceiptRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(receipt);
        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        var actual = await receiptService.ToPayAsync(receiptId);

        //assert
        _ = actual.Should().Be(expectedSum);
    }

    [TestCase(4, 30)]
    public async Task AddProductAsync_ProductNotAddedBefore_CreatesReceiptDetail(int productId, int discountValue)
    {
        //arrange
        var receipt = new Receipt
        {
            Id = 1,
            Customer = new Customer { Id = 1, DiscountValue = discountValue },
            ReceiptDetails =
            [
                new ReceiptDetail
                {
                    Id = 1,
                    ProductId = 1,
                    UnitPrice = 10,
                    DiscountUnitPrice = 9,
                    Quantity = 2,
                    ReceiptId = 1
                },
                new ReceiptDetail
                {
                    Id = 2,
                    ProductId = 2,
                    UnitPrice = 20,
                    DiscountUnitPrice = 19,
                    Quantity = 3,
                    ReceiptId = 1
                },
                new ReceiptDetail
                {
                    Id = 3,
                    ProductId = 3,
                    UnitPrice = 25,
                    DiscountUnitPrice = 24,
                    Quantity = 1,
                    ReceiptId = 1
                }
            ]
        };

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(x => x.ReceiptRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(receipt);
        _ = mockUnitOfWork.Setup(x => x.ProductRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(ProductEntities.FirstOrDefault(x => x.Id == productId));
        _ = mockUnitOfWork.Setup(x => x.ReceiptDetailRepository.AddAsync(It.IsAny<ReceiptDetail>()));
        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        await receiptService.AddProductAsync(productId, 1, 8);

        //assert
        mockUnitOfWork.Verify(x => x.ReceiptDetailRepository.AddAsync(It.Is<ReceiptDetail>(receiptDetail =>
            receiptDetail.ReceiptId == receipt.Id && receiptDetail.ProductId == productId &&
            receiptDetail.Quantity == 8)), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [TestCase(1, 5, 7)]
    [TestCase(2, 1, 4)]
    [TestCase(3, 200, 201)]
    public async Task AddProductAsync_ProductAlreadyInReceipt_UpdatesQuantity(int productId, int quantity,
        int expectedQuantity)
    {
        //arrange
        var receipt = new Receipt
        {
            Id = 2,
            ReceiptDetails =
            [
                new ReceiptDetail
                {
                    Id = 1,
                    ProductId = 1,
                    UnitPrice = 10,
                    DiscountUnitPrice = 9,
                    Quantity = 2,
                    ReceiptId = 1
                },
                new ReceiptDetail
                {
                    Id = 2,
                    ProductId = 2,
                    UnitPrice = 20,
                    DiscountUnitPrice = 19,
                    Quantity = 3,
                    ReceiptId = 1
                },
                new ReceiptDetail
                {
                    Id = 3,
                    ProductId = 3,
                    UnitPrice = 25,
                    DiscountUnitPrice = 24,
                    Quantity = 1,
                    ReceiptId = 1
                }
            ]
        };

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(x => x.ReceiptRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(receipt);
        _ = mockUnitOfWork.Setup(x => x.ReceiptDetailRepository.AddAsync(It.IsAny<ReceiptDetail>()));
        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        await receiptService.AddProductAsync(productId, 2, quantity);

        //assert
        var actualQuantity = receipt.ReceiptDetails.First(x => x.ProductId == productId).Quantity;
        _ = actualQuantity.Should().Be(expectedQuantity);
        mockUnitOfWork.Verify(x => x.ReceiptDetailRepository.AddAsync(It.IsAny<ReceiptDetail>()), Times.Never);
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }


    [TestCase(1, 15, 15.3)]
    [TestCase(2, 20, 15.2)]
    [TestCase(3, 50, 5.0)]
    [TestCase(8, 99, 0.38)]
    public async Task AddProductAsync_ValidDiscount_SetsDiscountUnitPrice(int productId, int discount,
        decimal expectedDiscountPrice)
    {
        //arrange
        var product = ProductEntities.FirstOrDefault(x => x.Id == productId);
        var receipt = new Receipt
        {
            Id = 1, CustomerId = 1, Customer = new Customer { Id = 1, DiscountValue = discount }
        };

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(x => x.ReceiptRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(receipt);
        _ = mockUnitOfWork.Setup(x => x.ProductRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(product);
        _ = mockUnitOfWork.Setup(x => x.ReceiptDetailRepository.AddAsync(It.IsAny<ReceiptDetail>()));

        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        await receiptService.AddProductAsync(productId, 1, 2);

        //assert
        mockUnitOfWork.Verify(x => x.ReceiptDetailRepository.AddAsync(It.Is<ReceiptDetail>(detail =>
            detail.ReceiptId == receipt.Id && detail.UnitPrice == product!.Price && detail.ProductId == product.Id &&
            detail.DiscountUnitPrice == expectedDiscountPrice)), Times.Once);

        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }


    [TestCase(1)]
    [TestCase(2)]
    public async Task AddProductAsync_ProductDoesNotExist_ThrowsMarketException(int productId)
    {
        //arrange
        var receipt = new Receipt { Id = 1, CustomerId = 1 };

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(x => x.ReceiptRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(receipt);
        _ = mockUnitOfWork.Setup(x => x.ProductRepository.GetByIdAsync(It.IsAny<int>()));

        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        Func<Task> act = async () => await receiptService.AddProductAsync(productId, 1, 1);

        //assert
        _ = await act.Should().ThrowAsync<MarketException>();
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task AddProductAsync_ReceiptDoesNotExist_ThrowsMarketException(int receiptId)
    {
        //arrange

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(x => x.ReceiptRepository.GetByIdWithDetailsAsync(It.IsAny<int>()));
        _ = mockUnitOfWork.Setup(x => x.ProductRepository.GetByIdAsync(It.IsAny<int>()));

        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        Func<Task> act = async () => await receiptService.AddProductAsync(1, receiptId, 1);

        //assert
        _ = await act.Should().ThrowAsync<MarketException>();
    }


    [Test]
    public async Task CheckOutAsync_ValidModel_UpdatesCheckOutPropertyAndSavesChanges()
    {
        //arrange
        var receipt = new Receipt { Id = 6, IsCheckedOut = false };
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(x => x.ReceiptRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(receipt);

        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        await receiptService.CheckOutAsync(6);

        //assert
        _ = receipt.IsCheckedOut.Should().BeTrue();
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }


    [TestCase(1, 1, 1)]
    [TestCase(2, 2, 1)]
    [TestCase(3, 3, 2)]
    public async Task RemoveProductAsync_ValidProduct_UpdatesDetailQuantityValue(int productId, int quantity,
        int expectedQuantity)
    {
        //arrange
        var receipt = new Receipt
        {
            Id = 1,
            ReceiptDetails =
            [
                new ReceiptDetail
                {
                    Id = 1,
                    ProductId = 1,
                    UnitPrice = 10,
                    DiscountUnitPrice = 9,
                    Quantity = 2,
                    ReceiptId = 1
                },
                new ReceiptDetail
                {
                    Id = 2,
                    ProductId = 2,
                    UnitPrice = 20,
                    DiscountUnitPrice = 19,
                    Quantity = 3,
                    ReceiptId = 1
                },
                new ReceiptDetail
                {
                    Id = 3,
                    ProductId = 3,
                    UnitPrice = 25,
                    DiscountUnitPrice = 24,
                    Quantity = 5,
                    ReceiptId = 1
                }
            ]
        };

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(x => x.ReceiptRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(receipt);
        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        await receiptService.RemoveProductAsync(productId, 1, quantity);

        //assert
        var actualQuantity = receipt.ReceiptDetails.First(x => x.ProductId == productId).Quantity;
        _ = actualQuantity.Should().Be(expectedQuantity);
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task RemoveProductAsync_QuantityEqualsZero_DeletesDetail()
    {
        //arrange
        var receipt = new Receipt
        {
            Id = 1,
            ReceiptDetails =
            [
                new ReceiptDetail
                {
                    Id = 1,
                    ProductId = 1,
                    UnitPrice = 10,
                    DiscountUnitPrice = 9,
                    Quantity = 2,
                    ReceiptId = 1
                },
                new ReceiptDetail
                {
                    Id = 2,
                    ProductId = 2,
                    UnitPrice = 20,
                    DiscountUnitPrice = 19,
                    Quantity = 3,
                    ReceiptId = 1
                },
                new ReceiptDetail
                {
                    Id = 3,
                    ProductId = 3,
                    UnitPrice = 25,
                    DiscountUnitPrice = 24,
                    Quantity = 1,
                    ReceiptId = 1
                }
            ]
        };

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(x => x.ReceiptRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(receipt);
        _ = mockUnitOfWork.Setup(x => x.ReceiptDetailRepository.Delete(It.IsAny<ReceiptDetail>()));
        var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        await receiptService.RemoveProductAsync(1, 1, 2);

        //assert
        mockUnitOfWork.Verify(
            x => x.ReceiptDetailRepository.Delete(It.Is<ReceiptDetail>(rd =>
                rd.ReceiptId == receipt.Id && rd.ProductId == 1)), Times.Once());
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }


    private static IEnumerable<Receipt> GetTestReceiptsEntities =>
        new List<Receipt>()
        {
            new Receipt
            {
                Id = 1,
                CustomerId = 1,
                IsCheckedOut = false,
                OperationDate = new DateTime(1965, 1, 2),
                ReceiptDetails =
                [
                    new ReceiptDetail
                    {
                        Id = 1,
                        ProductId = 1,
                        UnitPrice = 10,
                        DiscountUnitPrice = 9,
                        Quantity = 2,
                        ReceiptId = 1
                    },
                    new ReceiptDetail
                    {
                        Id = 2,
                        ProductId = 2,
                        UnitPrice = 20,
                        DiscountUnitPrice = 19,
                        Quantity = 3,
                        ReceiptId = 1
                    },
                    new ReceiptDetail
                    {
                        Id = 3,
                        ProductId = 3,
                        UnitPrice = 25,
                        DiscountUnitPrice = 24,
                        Quantity = 1,
                        ReceiptId = 1
                    }
                ]
            },
            new Receipt
            {
                Id = 2,
                CustomerId = 2,
                IsCheckedOut = false,
                OperationDate = new DateTime(1965, 1, 15),
                ReceiptDetails =
                [
                    new ReceiptDetail
                    {
                        Id = 4,
                        ProductId = 1,
                        UnitPrice = 10,
                        DiscountUnitPrice = 9,
                        Quantity = 10,
                        ReceiptId = 2
                    },
                    new ReceiptDetail
                    {
                        Id = 5,
                        ProductId = 3,
                        UnitPrice = 25,
                        DiscountUnitPrice = 24,
                        Quantity = 1,
                        ReceiptId = 2
                    }
                ]
            },
            new Receipt
            {
                Id = 3,
                CustomerId = 3,
                IsCheckedOut = false,
                OperationDate = new DateTime(1965, 2, 15),
                ReceiptDetails =
                [
                    new ReceiptDetail
                    {
                        Id = 6,
                        ProductId = 1,
                        UnitPrice = 10,
                        DiscountUnitPrice = 9,
                        Quantity = 10,
                        ReceiptId = 3
                    },
                    new ReceiptDetail
                    {
                        Id = 7,
                        ProductId = 2,
                        UnitPrice = 25,
                        DiscountUnitPrice = 24,
                        Quantity = 18,
                        ReceiptId = 3
                    }
                ]
            },
            new Receipt
            {
                Id = 4,
                CustomerId = 4,
                IsCheckedOut = false,
                OperationDate = new DateTime(1965, 2, 28),
                ReceiptDetails =
                [
                    new ReceiptDetail
                    {
                        Id = 8,
                        ProductId = 5,
                        UnitPrice = 10,
                        DiscountUnitPrice = 9,
                        Quantity = 10,
                        ReceiptId = 4
                    },
                    new ReceiptDetail
                    {
                        Id = 9,
                        ProductId = 6,
                        UnitPrice = 25,
                        DiscountUnitPrice = 24,
                        Quantity = 19,
                        ReceiptId = 4
                    }
                ]
            },
            new Receipt
            {
                Id = 5,
                CustomerId = 1,
                IsCheckedOut = true,
                OperationDate = new DateTime(1965, 2, 22),
                ReceiptDetails =
                [
                    new ReceiptDetail
                    {
                        Id = 10,
                        ProductId = 8,
                        UnitPrice = 20,
                        DiscountUnitPrice = 15,
                        Quantity = 30,
                        ReceiptId = 5
                    },
                    new ReceiptDetail
                    {
                        Id = 11,
                        ProductId = 9,
                        UnitPrice = 35,
                        DiscountUnitPrice = 26,
                        Quantity = 24,
                        ReceiptId = 5
                    }
                ]
            }
        };

    private static IEnumerable<ReceiptModel> GetTestReceiptsModels =>
        new List<ReceiptModel>()
        {
            new ReceiptModel
            {
                Id = 1,
                CustomerId = 1,
                IsCheckedOut = false,
                OperationDate = new DateTime(1965, 1, 2),
                ReceiptDetailsIds = [1, 2, 3]
            },
            new ReceiptModel
            {
                Id = 2,
                CustomerId = 2,
                IsCheckedOut = false,
                OperationDate = new DateTime(1965, 1, 15),
                ReceiptDetailsIds = [4, 5]
            },
            new ReceiptModel
            {
                Id = 3,
                CustomerId = 3,
                IsCheckedOut = false,
                OperationDate = new DateTime(1965, 2, 15),
                ReceiptDetailsIds = [6, 7]
            },
            new ReceiptModel
            {
                Id = 4,
                CustomerId = 4,
                IsCheckedOut = false,
                OperationDate = new DateTime(1965, 2, 28),
                ReceiptDetailsIds = [8, 9]
            },
            new ReceiptModel
            {
                Id = 5,
                CustomerId = 1,
                IsCheckedOut = true,
                OperationDate = new DateTime(1965, 2, 22),
                ReceiptDetailsIds = [10, 11]
            }
        };


    private static IEnumerable<Product> ProductEntities =>
        new List<Product>
        {
            new Product
            {
                Id = 1,
                ProductName = "Chai",
                ProductCategoryId = 1,
                Category = new ProductCategory { Id = 1, CategoryName = "Beverages" },
                Price = 18.00m
            },
            new Product
            {
                Id = 2,
                ProductName = "Chang",
                ProductCategoryId = 1,
                Category = new ProductCategory { Id = 1, CategoryName = "Beverages" },
                Price = 19.00m
            },
            new Product
            {
                Id = 3,
                ProductName = "Aniseed Syrup",
                ProductCategoryId = 2,
                Category = new ProductCategory { Id = 2, CategoryName = "Condiments" },
                Price = 10.00m
            },
            new Product
            {
                Id = 4,
                ProductName = "Chef Anton's Cajun Seasoning",
                ProductCategoryId = 2,
                Category = new ProductCategory { Id = 2, CategoryName = "Condiments" },
                Price = 22.00m
            },
            new Product
            {
                Id = 5,
                ProductName = "Grandma's Boysenberry Spread",
                ProductCategoryId = 3,
                Category = new ProductCategory { Id = 3, CategoryName = "Confections" },
                Price = 25.00m
            },
            new Product
            {
                Id = 6,
                ProductName = "Uncle Bob's Organic Dried Pears",
                ProductCategoryId = 4,
                Category = new ProductCategory { Id = 4, CategoryName = "Dairy Products" },
                Price = 14.60m
            },
            new Product
            {
                Id = 7,
                ProductName = "Queso Cabrales",
                ProductCategoryId = 4,
                Category = new ProductCategory { Id = 4, CategoryName = "Dairy Products" },
                Price = 21.00m
            },
            new Product
            {
                Id = 8,
                ProductName = "Queso Manchego La Pastora",
                ProductCategoryId = 3,
                Category = new ProductCategory { Id = 3, CategoryName = "Confections" },
                Price = 38.00m
            },
            new Product
            {
                Id = 9,
                ProductName = "Tofu",
                ProductCategoryId = 2,
                Category = new ProductCategory { Id = 2, CategoryName = "Condiments" },
                Price = 15.50m
            }
        };
}
