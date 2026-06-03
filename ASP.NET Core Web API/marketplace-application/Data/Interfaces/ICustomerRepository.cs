using Data.Entities;

namespace Data.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<IEnumerable<Customer>> GetAllWithDetailsAsync();

    Task<Customer?> GetByIdWithDetailsAsync(int id);
}
