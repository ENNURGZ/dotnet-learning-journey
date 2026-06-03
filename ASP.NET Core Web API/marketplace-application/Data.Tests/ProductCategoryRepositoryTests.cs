using Data.Data;
using Data.Entities;
using Data.Repositories;
using Data.Tests.Comparers;

namespace Data.Tests;

[TestFixture]
public class ProductCategoryRepositoryTests
{
    [TestCase(1)]
    [TestCase(2)]
    public async Task GetByIdAsync_WhenValidId_ReturnsProductCategory(int id)
    {
        await using var context = new TradeMarketDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var productCategoryRepository = new CategoryRepository(context);
        var productCategory = await productCategoryRepository.GetByIdAsync(id);

        var expected = ExpectedProductCategories.FirstOrDefault(x => x.Id == id);

        Assert.That(productCategory, Is.EqualTo(expected).Using(new ProductCategoryEqualityComparer()), message: "GetByIdAsync method works incorrect");
    }

    [Test]
    public async Task GetAllAsync_WhenCalled_ReturnsAllProductCategories()
    {
        await using var context = new TradeMarketDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var productCategoryRepository = new CategoryRepository(context);
        var productCategories = await productCategoryRepository.GetAllAsync();

        Assert.That(productCategories, Is.EqualTo(ExpectedProductCategories).Using(new ProductCategoryEqualityComparer()), message: "GetAllAsync method works incorrect");
    }

    [Test]
    public async Task AddAsync_WhenValidProductCategory_AddsProductCategoryToDatabase()
    {
        await using var context = new TradeMarketDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var productCategoryRepository = new CategoryRepository(context);
        var productCategory = new ProductCategory 
        { 
            Id = 3,
            CategoryName = "Test Category"
        };

        await productCategoryRepository.AddAsync(productCategory);
        _ = await context.SaveChangesAsync();

        Assert.That(context.ProductCategories.Count(), Is.EqualTo(3), message: "AddAsync method works incorrect");
    }

    [Test]
    public async Task DeleteByIdAsync_WhenValidId_DeletesProductCategoryFromDatabase()
    {
        await using var context = new TradeMarketDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var productCategoryRepository = new CategoryRepository(context);

        await productCategoryRepository.DeleteByIdAsync(1);
        _ = await context.SaveChangesAsync();

        Assert.That(context.ProductCategories.Count(), Is.EqualTo(1), message: "DeleteByIdAsync works incorrect");
    }

    [Test]
    public async Task Update_WhenValidProductCategory_UpdatesProductCategoryInDatabase()
    {
        await using var context = new TradeMarketDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var productCategoryRepository = new CategoryRepository(context);
        var productCategory = new ProductCategory
        {
            Id = 1,
            CategoryName = "Dairy food"
        };

        productCategoryRepository.Update(productCategory);
        _ = await context.SaveChangesAsync();

        Assert.That(productCategory, Is.EqualTo(new ProductCategory
        {
            Id = 1,
            CategoryName = "Dairy food"
        }).Using(new ProductCategoryEqualityComparer()), message: "Update method works incorrect");
    }

    private static IEnumerable<ProductCategory> ExpectedProductCategories =>
        new[]
        {
            new ProductCategory { Id = 1, CategoryName = "Dairy products" },
            new ProductCategory { Id = 2, CategoryName = "Fruit juices" }
        };
}
