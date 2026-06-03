using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ReceiptDetailRepository : Repository<ReceiptDetail>, IReceiptDetailRepository
{
    public ReceiptDetailRepository(TradeMarketDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
    {
        return await this.Context.ReceiptsDetails
            .Include(rd => rd.Receipt)
            .Include(rd => rd.Product)
                .ThenInclude(p => p.Category)
            .ToListAsync();
    }
}