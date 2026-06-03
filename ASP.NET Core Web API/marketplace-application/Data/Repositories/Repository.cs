using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    protected Repository(TradeMarketDbContext context)
    {
        this.Context = context;
    }

    protected TradeMarketDbContext Context { get; }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await this.Context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await this.Context.Set<TEntity>().FindAsync(id);
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await this.Context.Set<TEntity>().AddAsync(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        this.Context.Set<TEntity>().Remove(entity);
    }

    public virtual async Task DeleteByIdAsync(int id)
    {
        var entity = await this.GetByIdAsync(id);
        if (entity != null)
        {
            this.Delete(entity);
        }
    }

    public virtual void Update(TEntity entity)
    {
        this.Context.Set<TEntity>().Update(entity);
    }
}
