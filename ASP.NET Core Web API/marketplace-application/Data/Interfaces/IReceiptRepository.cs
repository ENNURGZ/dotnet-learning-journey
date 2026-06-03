using Data.Entities;

namespace Data.Interfaces;

public interface IReceiptRepository : IRepository<Receipt>
{
    Task<IEnumerable<Receipt>> GetAllWithDetailsAsync();

    Task<Receipt?> GetByIdWithDetailsAsync(int id);
}
