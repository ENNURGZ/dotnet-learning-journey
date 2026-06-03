using Business.Models;

namespace Business.Interfaces;

public interface IStatisticService
{
    Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount);

    Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId);

    Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate);

    Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate);
}
