using Business.Models;

namespace Business.Interfaces;

public interface ICustomerService : IModelService<CustomerModel>
{
    Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId);
}
