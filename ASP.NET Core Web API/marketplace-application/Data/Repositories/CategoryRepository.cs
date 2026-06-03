using Data.Data;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class CategoryRepository : Repository<ProductCategory>, ICategoryRepository
{
    public CategoryRepository(TradeMarketDbContext context)
        : base(context)
    {
    }
}