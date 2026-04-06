using ShopReports.Models;
using ShopReports.Reports;

namespace ShopReports.Services;

public class ProductReportService : IDisposable
{
    private readonly ShopContext shopContext;

    public ProductReportService(ShopContext shopContext)
    {
        this.shopContext = shopContext;
    }

    public ProductCategoryReport GetProductCategoryReport()
    {
        var lines = this.shopContext.Categories
            .OrderBy(c => c.Name)
            .Select(c => new ProductCategoryReportLine
            {
                CategoryId = c.Id,
                CategoryName = c.Name,
            })
            .ToList();

        return new ProductCategoryReport(lines, DateTime.Now);
    }

    public ProductReport GetProductReport()
    {
        var lines = this.shopContext.Products
            .OrderByDescending(p => p.Title.Title)
            .Select(p => new ProductReportLine
            {
                ProductId = p.Id,
                ProductTitle = p.Title.Title,
                Manufacturer = p.Manufacturer.Name,
                Price = p.UnitPrice,
            })
            .ToList();

        return new ProductReport(lines, DateTime.Now);
    }

    public FullProductReport GetFullProductReport()
    {
        var lines = this.shopContext.Products
            .OrderBy(p => p.Title.Title)
            .Select(p => new FullProductReportLine
            {
                ProductId = p.Id,
                Name = p.Title.Title,
                CategoryId = p.Title.Category.Id,
                Category = p.Title.Category.Name,
                Manufacturer = p.Manufacturer.Name,
                Price = p.UnitPrice,
            })
            .ToList();

        return new FullProductReport(lines, DateTime.Now);
    }

    public ProductTitleSalesRevenueReport GetProductTitleSalesRevenueReport()
    {
        var lines = this.shopContext.Titles
            .Select(t => new ProductTitleSalesRevenueReportLine
            {
                ProductTitleName = t.Title,
                SalesRevenue = t.Products.SelectMany(p => p.OrderDetails).Sum(od => od.PriceWithDiscount),
                SalesAmount = t.Products.SelectMany(p => p.OrderDetails).Sum(od => od.ProductAmount),
            })
            .Where(l => l.SalesAmount > 0)
            .OrderByDescending(l => l.SalesRevenue)

            .ToList();

        return new ProductTitleSalesRevenueReport(lines, DateTime.Now);
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
