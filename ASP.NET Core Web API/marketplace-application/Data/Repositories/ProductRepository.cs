using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(TradeMarketDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
    {
        return await this.Context.Products
            .Include(p => p.Category)
            .Include(p => p.ReceiptDetails)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdWithDetailsAsync(int id)
    {
        return await this.Context.Products
            .Include(p => p.Category)
            .Include(p => p.ReceiptDetails)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}