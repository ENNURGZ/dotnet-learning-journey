using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ReceiptRepository : Repository<Receipt>, IReceiptRepository
{
    public ReceiptRepository(TradeMarketDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
    {
        return await this.Context.Receipts
            .Include(r => r.Customer)
            .Include(r => r.ReceiptDetails!)
                .ThenInclude(rd => rd.Product)
                    .ThenInclude(p => p.Category)
            .ToListAsync();
    }

    public async Task<Receipt?> GetByIdWithDetailsAsync(int id)
    {
        return await this.Context.Receipts
            .Include(r => r.Customer)
            .Include(r => r.ReceiptDetails!)
                .ThenInclude(rd => rd.Product)
                    .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}