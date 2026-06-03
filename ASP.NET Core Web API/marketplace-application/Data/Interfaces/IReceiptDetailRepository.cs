using Data.Entities;

namespace Data.Interfaces;

public interface IReceiptDetailRepository : IRepository<ReceiptDetail>
{
    Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync();
}
