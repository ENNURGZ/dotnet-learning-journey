using BankSystem.Services.Services;
using BankSystem.Tests.Entities;
using NUnit.Framework;

namespace BankSystem.Tests.Services;

[TestFixture]
public sealed class AccountServiceTests : ServiceBaseTests<AccountService>
{
    private static readonly object[][] TestContainer = new object[][]
    {
        new object[]
        {
            "GetBankAccountsInfo", (AccountService service) => service.GetBankAccountsFullInfo(), (BankContextFactory factory) => factory.GetEntitiesBySqlQueryName(
                "GetBankAccountsInfo",
                ModelBuilders.BuildBankAccountFullInfoModel),
        },
    };

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();
#pragma warning disable CA2000
        this.Service = new AccountService(this.Factory.CreateContext());
    }

    [TearDown]
    public void TearDown()
    {
        this.Service.Dispose();
    }

    [TestCaseSource(nameof(TestContainer))]
    public new void ReportService_ReturnsCorrectReportLines(
        string methodName,
        Func<AccountService, IReadOnlyList<object>> getActualLines,
        Func<BankContextFactory, IReadOnlyList<object>> getExpectedLines)
    {
        base.ReportService_ReturnsCorrectReportLines(methodName, getActualLines, getExpectedLines);
    }
}
