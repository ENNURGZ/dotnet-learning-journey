using ShopReports.Models;
using ShopReports.Reports;

namespace ShopReports.Services;

public class CustomerReportService : IDisposable
{
    private readonly ShopContext shopContext;

    public CustomerReportService(ShopContext shopContext)
    {
        this.shopContext = shopContext;
    }

    public CustomerSalesRevenueReport GetCustomerSalesRevenueReport()
    {
        var lines = this.shopContext.Customers
            .Where(c => c.Orders.Any())
            .Select(c => new CustomerSalesRevenueReportLine
            {
                CustomerId = c.Id,
                PersonFirstName = c.Person.FirstName,
                PersonLastName = c.Person.LastName,
                SalesRevenue = c.Orders.SelectMany(o => o.Details).Sum(od => od.PriceWithDiscount),
            })
            .OrderByDescending(l => l.SalesRevenue)

            .ToList();

        return new CustomerSalesRevenueReport(lines, DateTime.Now);
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
            this.shopContext.Dispose();
        }
    }
}
