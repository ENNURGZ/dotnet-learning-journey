using NUnit.Framework;
using ShopReports.Services;

namespace ShopReports.Tests.Services;

[TestFixture]
public sealed class ProductReportServiceTests : ReportServiceBaseTests<ProductReportService>
{
    private static readonly object[][] TestContainer = new object[][]
    {
        new object[] { "GetProductCategoryReport", (ProductReportService service) => service.GetProductCategoryReport().Lines, (ShopContextFactory factory) => factory.GetEntitiesBySqlQueryName("GetProductCategoryReport", ReportBuilders.BuildProductCategoryReportLine), },
        new object[] { "GetProductReport", (ProductReportService service) => service.GetProductReport().Lines, (ShopContextFactory factory) => factory.GetEntitiesBySqlQueryName("GetProductReport", ReportBuilders.BuildProductReportLine), },
        new object[] { "GetFullProductReport", (ProductReportService service) => service.GetFullProductReport().Lines, (ShopContextFactory factory) => factory.GetEntitiesBySqlQueryName("GetFullProductReport", ReportBuilders.BuildFullProductReportLine), },
        new object[] { "GetProductTitleSalesRevenueReport", (ProductReportService service) => service.GetProductTitleSalesRevenueReport().Lines, (ShopContextFactory factory) => factory.GetEntitiesBySqlQueryName("GetProductTitleSalesRevenueReport", ReportBuilders.BuildProductTitleSalesRevenueReportLine), },
    };

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();
        #pragma warning disable CA2000
        this.Service = new ProductReportService(this.Factory.CreateContext());
        #pragma warning disable CA2000
    }

    [TearDown]
    public void TearDown()
    {
        this.Service.Dispose();
    }

    [TestCaseSource(nameof(TestContainer))]
    public new void ReportService_ReturnsCorrectReportLines(string methodName, Func<ProductReportService, IReadOnlyList<object>> getActualLines, Func<ShopContextFactory, IReadOnlyList<object>> getExpectedLines)
    {
        base.ReportService_ReturnsCorrectReportLines(methodName, getActualLines, getExpectedLines);
    }
}
