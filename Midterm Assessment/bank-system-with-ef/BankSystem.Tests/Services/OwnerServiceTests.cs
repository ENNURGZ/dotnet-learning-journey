using BankSystem.Services.Services;
using BankSystem.Tests.Entities;
using NUnit.Framework;

namespace BankSystem.Tests.Services;

[TestFixture]
public sealed class OwnerServiceTests : ServiceBaseTests<OwnerService>
{
    private static readonly object[][] TestContainer = new object[][]
    {
        new object[]
        {
            "GetProductCategoryReport", (OwnerService service) => service.GetAccountOwnersTotalBalance(),
            (BankContextFactory factory) => factory.GetEntitiesBySqlQueryName(
                "GetProductCategoryReport",
                ModelBuilders.BuildAcountOwnerTotalBalanceModel),
        },
    };

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();
#pragma warning disable CA2000
        this.Service = new OwnerService(this.Factory.CreateContext());
    }

    [TearDown]
    public void TearDown()
    {
        this.Service.Dispose();
    }

    [TestCaseSource(nameof(TestContainer))]
    public new void ReportService_ReturnsCorrectReportLines(
        string methodName,
        Func<OwnerService, IReadOnlyList<object>> getActualLines,
        Func<BankContextFactory, IReadOnlyList<object>> getExpectedLines)
    {
        base.ReportService_ReturnsCorrectReportLines(methodName, getActualLines, getExpectedLines);
    }
}
