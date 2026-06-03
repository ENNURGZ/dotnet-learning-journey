using Business.Models;
using Business.Services;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using FluentAssertions;
using Moq;

namespace Business.Tests;

public class ProductServiceTests
{
    [Test]
    public async Task GetAllAsync_Always_ReturnsAllProducts()
    {
        //arrange
        var expected = ProductModels.ToList();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        _ = mockUnitOfWork
            .Setup(x => x.ProductRepository.GetAllWithDetailsAsync())
            .ReturnsAsync(ProductEntities.AsEnumerable());

        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        var actual = await productService.GetAllAsync();

        //assert
        _ = actual.Should().BeEquivalentTo(expected, options =>
            options.Excluding(x => x.ReceiptDetailIds));
    }

    [Test]
    public async Task GetAllProductCategoriesAsync_Always_ReturnsAllCategories()
    {
        //arrange
        var expected = ProductCategoryModels;
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        _ = mockUnitOfWork
            .Setup(x => x.CategoryRepository.GetAllAsync())
            .ReturnsAsync(ProductCategoryEntities.AsEnumerable());

        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        var actual = await productService.GetAllProductCategoriesAsync();

        //assert
        _ = actual.Should().BeEquivalentTo(expected, options => options.Excluding(x => x.ProductIds));
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task GetByIdAsync_ExistingId_ReturnsProductModel(int id)
    {
        //arrange
        var expected = ProductModels.FirstOrDefault(x => x.Id == id);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork
            .Setup(x => x.ProductRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(ProductEntities.FirstOrDefault(x => x.Id == id));

        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        var actual = await productService.GetByIdAsync(id);

        //assert
        _ = actual.Should().BeEquivalentTo(expected, options => options.Excluding(x => x!.ReceiptDetailIds));
    }


    [Test]
    public async Task AddAsync_ValidModel_AddsProduct()
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.ProductRepository.AddAsync(It.IsAny<Product>()));

        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var product = new ProductModel { Id = 11, ProductName = "Orange juice", ProductCategoryId = 9, Price = 29.00m };

        //act
        await productService.AddAsync(product);

        //assert
        mockUnitOfWork.Verify(x => x.ProductRepository.AddAsync(It.Is<Product>(c => c.Id == product.Id && c.ProductCategoryId == product.ProductCategoryId && c.Price == product.Price && c.ProductName == product.ProductName)), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task AddCategoryAsync_ValidModel_AddsCategory()
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.CategoryRepository.AddAsync(It.IsAny<ProductCategory>()));

        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var category = new CategoryModel { Id = 98, Name = "Foodstuff" };

        //act
        await productService.AddCategoryAsync(category);

        //add equality comparer
        //assert
        mockUnitOfWork.Verify(x => x.CategoryRepository.AddAsync(It.Is<ProductCategory>(c => c.Id == category.Id && c.CategoryName == category.Name)), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task AddAsync_EmptyProductName_ThrowsMarketException()
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.ProductRepository.AddAsync(It.IsAny<Product>()));

        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var product = new ProductModel { Id = 1, ProductName = string.Empty, ProductCategoryId = 1, CategoryName = "Beverages", Price = 18.00m };

        //act
        Func<Task> act = async () => await productService.AddAsync(product);

        //assert
        _ = await act.Should().ThrowAsync<MarketException>();
    }

    [TestCase(-5000.50)]
    [TestCase(-500000)]
    [TestCase(-0.0001)]
    public async Task AddAsync_NegativePrice_ThrowsMarketException(decimal price)
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.ProductRepository.AddAsync(It.IsAny<Product>()));

        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var product = new ProductModel { Id = 1, ProductName = "Cola", ProductCategoryId = 1, CategoryName = "Beverages", Price = price };

        //act
        Func<Task> act = async () => await productService.AddAsync(product);

        //assert
        _ = await act.Should().ThrowAsync<MarketException>();
    }


    [Test]
    public async Task AddCategoryAsync_EmptyCategoryName_ThrowsMarketException()
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.CategoryRepository.AddAsync(It.IsAny<ProductCategory>()));

        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var category = new CategoryModel { Id = 10, Name = "" };

        //act
        Func<Task> act = async () => await productService.AddCategoryAsync(category);

        //assert
        _ = await act.Should().ThrowAsync<MarketException>();
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task DeleteAsync_ExistingId_DeletesProduct(int id)
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.ProductRepository.DeleteByIdAsync(It.IsAny<int>()));
        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        await productService.DeleteAsync(id);

        //assert
        mockUnitOfWork.Verify(x => x.ProductRepository.DeleteByIdAsync(id), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task RemoveCategoryAsync_ExistingId_DeletesCategory(int id)
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.CategoryRepository.DeleteByIdAsync(It.IsAny<int>()));
        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        await productService.RemoveCategoryAsync(id);

        //assert
        mockUnitOfWork.Verify(x => x.CategoryRepository.DeleteByIdAsync(id), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_ValidModel_UpdatesProduct()
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.ProductRepository.Update(It.IsAny<Product>()));

        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var product = new ProductModel { Id = 9, ProductName = "Cabrales", ProductCategoryId = 45, CategoryName = "Household", Price = 29.00m };

        //act
        await productService.UpdateAsync(product);

        //assert
        mockUnitOfWork.Verify(x => x.ProductRepository.Update(It.Is<Product>(c => c.Id == product.Id && c.ProductCategoryId == product.ProductCategoryId && c.Price == product.Price && c.ProductName == product.ProductName)), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_EmptyProductName_ThrowsMarketException()
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.ProductRepository.Update(It.IsAny<Product>()));

        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var product = new ProductModel { Id = 3, ProductName = "", ProductCategoryId = 1, CategoryName = "Dairy Products", Price = 288.00m };

        //act
        Func<Task> act = async () => await productService.UpdateAsync(product);

        //assert
        _ = await act.Should().ThrowAsync<MarketException>();
    }

    [Test]
    public async Task UpdateCategoryAsync_ValidModel_UpdatesCategory()
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.CategoryRepository.Update(It.IsAny<ProductCategory>()));

        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var category = new CategoryModel { Id = 77, Name = "Name" };

        //act
        await productService.UpdateCategoryAsync(category);

        //assert
        mockUnitOfWork.Verify(x => x.CategoryRepository.Update(It.Is<ProductCategory>(c => c.Id == category.Id && category.Name == c.CategoryName)), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateCategoryAsync_EmptyCategoryName_ThrowsMarketException()
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.CategoryRepository.Update(It.IsAny<ProductCategory>()));

        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var category = new CategoryModel { Id = 77, Name = "" };

        //act
        Func<Task> act = async () => await productService.UpdateCategoryAsync(category);

        //assert
        _ = await act.Should().ThrowAsync<MarketException>();
    }


    private static readonly int[] CategoryId4ExpectedProducts = [6, 7];
    private static readonly int[] CategoryId5ExpectedProducts = [10, 11];
    private static readonly int[] CategoryId6ExpectedProducts = [12, 13];

    private static IEnumerable<object[]> GetByFilterAsync_CategoryId_TestCases =>
    [
        [4, CategoryId4ExpectedProducts],
        [5, CategoryId5ExpectedProducts],
        [6, CategoryId6ExpectedProducts],
    ];

    [TestCaseSource(nameof(GetByFilterAsync_CategoryId_TestCases))]
    public async Task GetByFilterAsync_CategoryId_ReturnsProductsByCategory(int categoryId, IEnumerable<int> expectedProductIds)
    {
        //arrange
        var expected = ProductModels.Where(x => expectedProductIds.Contains(x.Id));
        var filter = new FilterSearchModel { CategoryId = categoryId };

        var mockUnitOfWork = new Mock<IUnitOfWork>();

        _ = mockUnitOfWork
            .Setup(x => x.ProductRepository.GetAllWithDetailsAsync())
            .ReturnsAsync(ProductEntities.AsEnumerable());

        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        var actual = await productService.GetByFilterAsync(filter);

        //assert
        _ = actual.Should().BeEquivalentTo(expected, options => options.Excluding(x => x.ReceiptDetailIds));
    }

    private static readonly int[] MinPrice20ExpectedProducts = [4, 5, 7, 8, 10, 12, 13];
    private static readonly int[] MinPrice37ExpectedProducts = [8, 12, 13];

    private static IEnumerable<object[]> GetByFilterAsync_MinPrice_TestCases =>
    [
        [20, MinPrice20ExpectedProducts],
        [37, MinPrice37ExpectedProducts],
    ];

    [TestCaseSource(nameof(GetByFilterAsync_MinPrice_TestCases))]
    public async Task GetByFilterAsync_MinPrice_ReturnsProductsByMinPrice(int minPrice, IEnumerable<int> expectedProductIds)
    {
        //arrange
        var expected = ProductModels.Where(x => expectedProductIds.Contains(x.Id));
        var filter = new FilterSearchModel { MinPrice = minPrice };

        var mockUnitOfWork = new Mock<IUnitOfWork>();

        _ = mockUnitOfWork
            .Setup(x => x.ProductRepository.GetAllWithDetailsAsync())
            .ReturnsAsync(ProductEntities.AsEnumerable());

        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        var actual = await productService.GetByFilterAsync(filter);

        //assert
        _ = actual.Should().BeEquivalentTo(expected, options => options.Excluding(x => x.ReceiptDetailIds));
    }

    private static readonly int[] ComplexFilter1ExpectedProducts = [7];
    private static readonly int[] ComplexFilter2ExpectedProducts = [4, 9];
    private static readonly int[] ComplexFilter3ExpectedProducts = [12];
    private static readonly int[] ComplexFilter4ExpectedProducts = [10];

    private static IEnumerable<object[]> GetByFilterAsync_ComplexFilter_TestCases =>
    [
        [4, 16, -1, ComplexFilter1ExpectedProducts],
        [2, 15, 25, ComplexFilter2ExpectedProducts],
        [6, 30, 40, ComplexFilter3ExpectedProducts],
        [5, 10, 22, ComplexFilter4ExpectedProducts],
    ];

    [TestCaseSource(nameof(GetByFilterAsync_ComplexFilter_TestCases))]
    public async Task GetByFilterAsync_ComplexFilter_ReturnsProductsByFilter(int? categoryId, int? minPrice, int? maxPrice, IEnumerable<int> expectedProductIds)
    {
        //arrange
        var expected = ProductModels.Where(x => expectedProductIds.Contains(x.Id)).ToList();
        var filter = new FilterSearchModel
        {
            CategoryId = categoryId,
            MinPrice = minPrice,
            MaxPrice = maxPrice == -1 ? null : maxPrice
        };

        var mockUnitOfWork = new Mock<IUnitOfWork>();

        _ = mockUnitOfWork
            .Setup(x => x.ProductRepository.GetAllWithDetailsAsync())
            .ReturnsAsync(ProductEntities.AsEnumerable());

        var productService = new ProductService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        var actual = await productService.GetByFilterAsync(filter);

        //assert
        _ = actual.Should().BeEquivalentTo(expected, options => options.Excluding(x => x.ReceiptDetailIds));
    }

    #region Test Data

    private static IEnumerable<ProductModel> ProductModels =>
        new List<ProductModel>
        {
            new ProductModel { Id = 1, ProductName = "Chai", ProductCategoryId = 1, CategoryName =  "Beverages", Price = 18.00m },
            new ProductModel { Id = 2, ProductName = "Chang", ProductCategoryId = 1, CategoryName =  "Beverages", Price = 19.00m },
            new ProductModel { Id = 3, ProductName = "Aniseed Syrup", ProductCategoryId = 2,  CategoryName = "Condiments", Price = 10.00m },
            new ProductModel { Id = 4, ProductName = "Chef Anton's Cajun Seasoning", ProductCategoryId = 2,  CategoryName = "Condiments", Price = 22.00m },
            new ProductModel { Id = 5, ProductName = "Grandma's Boysenberry Spread", ProductCategoryId = 3, CategoryName = "Confections", Price = 25.00m },
            new ProductModel { Id = 6, ProductName = "Uncle Bob's Organic Dried Pears", ProductCategoryId = 4,  CategoryName = "Dairy Products", Price = 14.60m },
            new ProductModel { Id = 7, ProductName = "Queso Cabrales", ProductCategoryId = 4, CategoryName = "Dairy Products", Price = 21.00m },
            new ProductModel { Id = 8, ProductName = "Queso Manchego La Pastora", ProductCategoryId = 3, CategoryName = "Confections", Price = 38.00m },
            new ProductModel { Id = 9, ProductName = "Tofu", ProductCategoryId = 2, CategoryName = "Condiments", Price = 15.50m },
            new ProductModel { Id = 10, ProductName = "Gustaf's Knäckebröd", ProductCategoryId = 5, CategoryName = "Grains", Price = 21.00m },
            new ProductModel { Id = 11, ProductName = "Tunnbröd", ProductCategoryId = 5, CategoryName = "Grains", Price = 9.00m },
            new ProductModel { Id = 12, ProductName = "Alice Mutton", ProductCategoryId = 6, CategoryName = "Meat", Price = 39.00m },
            new ProductModel { Id = 13, ProductName = "Thüringer Rostbratwurst", ProductCategoryId = 6, CategoryName = "Meat", Price = 123.79m}

        };

    private static IEnumerable<Product> ProductEntities =>
        new List<Product>
        {
            new Product {Id = 1, ProductName = "Chai", ProductCategoryId = 1, Category = new ProductCategory { Id = 1, CategoryName = "Beverages" }, Price = 18.00m },
            new Product {Id = 2, ProductName = "Chang", ProductCategoryId = 1, Category = new ProductCategory { Id = 1, CategoryName = "Beverages" }, Price = 19.00m },
            new Product {Id = 3, ProductName = "Aniseed Syrup", ProductCategoryId = 2, Category = new ProductCategory { Id = 2, CategoryName = "Condiments" }, Price = 10.00m },
            new Product {Id = 4, ProductName = "Chef Anton's Cajun Seasoning", ProductCategoryId = 2, Category = new ProductCategory { Id = 2, CategoryName = "Condiments" }, Price = 22.00m },
            new Product {Id = 5, ProductName = "Grandma's Boysenberry Spread", ProductCategoryId = 3, Category = new ProductCategory { Id = 3, CategoryName = "Confections" }, Price = 25.00m },
            new Product {Id = 6, ProductName = "Uncle Bob's Organic Dried Pears", ProductCategoryId = 4, Category = new ProductCategory { Id = 4, CategoryName = "Dairy Products" }, Price = 14.60m },
            new Product {Id = 7, ProductName = "Queso Cabrales", ProductCategoryId = 4, Category = new ProductCategory { Id = 4, CategoryName = "Dairy Products" }, Price = 21.00m },
            new Product {Id = 8, ProductName = "Queso Manchego La Pastora", ProductCategoryId = 3, Category = new ProductCategory { Id = 3, CategoryName = "Confections" }, Price = 38.00m },
            new Product {Id = 9, ProductName = "Tofu", ProductCategoryId = 2, Category = new ProductCategory { Id = 2, CategoryName = "Condiments" }, Price = 15.50m },
            new Product {Id = 10, ProductName = "Gustaf's Knäckebröd", ProductCategoryId = 5, Category = new ProductCategory { Id = 5, CategoryName = "Grains" }, Price = 21.00m },
            new Product {Id = 11, ProductName = "Tunnbröd", ProductCategoryId = 5, Category = new ProductCategory { Id = 5, CategoryName = "Grains" }, Price = 9.00m },
            new Product {Id = 12, ProductName = "Alice Mutton", ProductCategoryId = 6, Category = new ProductCategory { Id = 6, CategoryName = "Meat" }, Price = 39.00m },
            new Product {Id = 13, ProductName = "Thüringer Rostbratwurst", ProductCategoryId = 6, Category = new ProductCategory { Id = 6, CategoryName = "Meat" }, Price = 123.79m}
        };

    private static IEnumerable<ProductCategory> ProductCategoryEntities =>
        new List<ProductCategory>
        {
            new ProductCategory
            {
                Id = 1, CategoryName = "Beverages",
            },
            new ProductCategory
            {
                Id = 2, CategoryName = "Condiments",
            },
            new ProductCategory
            {
                Id = 3, CategoryName = "Confections",
            },
            new ProductCategory
            {
                Id = 4, CategoryName = "Dairy Products",
            },
            new ProductCategory
            {
                Id = 5, CategoryName = "Grains",
            },
            new ProductCategory
            {
                Id = 6, CategoryName = "Meat",
            }
        };

    private static IEnumerable<CategoryModel> ProductCategoryModels =>
        new List<CategoryModel>
        {
            new CategoryModel { Id = 1, Name = "Beverages" },
            new CategoryModel { Id = 2, Name = "Condiments" },
            new CategoryModel { Id = 3, Name = "Confections" },
            new CategoryModel { Id = 4, Name = "Dairy Products" },
            new CategoryModel { Id = 5, Name = "Grains" },
            new CategoryModel { Id = 6, Name = "Meat" }
        };

    #endregion
}
