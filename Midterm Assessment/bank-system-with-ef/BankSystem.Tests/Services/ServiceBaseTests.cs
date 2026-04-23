using BankSystem.Tests.Entities;

namespace BankSystem.Tests.Services;

public class ServiceBaseTests<T> : IDisposable
    where T : class
{
    protected BankContextFactory Factory { get; private set; } = null!;

    protected T Service { get; set; } = null!;

    public void SetUp()
    {
        this.Factory = new BankContextFactory();
    }

    public void ReportService_ReturnsCorrectReportLines(string methodName, Func<T, IReadOnlyList<object>> getActualLines, Func<BankContextFactory, IReadOnlyList<object>> getExpectedLines)
    {
        // Arrange
        _ = getExpectedLines(this.Factory);

        // Act
        _ = getActualLines(this.Service);

        // Assert
        _ = $"Class: ProductReportService {Environment.NewLine}  Method {methodName} returns report";
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.Factory!.Dispose();
        }
    }
}
