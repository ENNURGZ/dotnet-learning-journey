using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(TradeMarketDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
    {
        return await this.Context.Customers
            .Include(c => c.Person)
            .Include(c => c.Receipts)
                .ThenInclude(r => r.ReceiptDetails)
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdWithDetailsAsync(int id)
    {
        return await this.Context.Customers
            .Include(c => c.Person)
            .Include(c => c.Receipts)
                .ThenInclude(r => r.ReceiptDetails)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}