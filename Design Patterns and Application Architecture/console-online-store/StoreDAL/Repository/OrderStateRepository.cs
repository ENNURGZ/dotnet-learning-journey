namespace StoreDAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

public class OrderStateRepository : AbstractRepository, IOrderStateRepository
{
    private readonly DbSet<OrderState> dbSet;

    public OrderStateRepository(StoreDbContext context)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.dbSet = context.Set<OrderState>();
    }

    public void Add(OrderState entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        this.dbSet.Add(entity);
        this.context.SaveChanges();
    }

    public void Delete(OrderState entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        this.dbSet.Remove(entity);
        this.context.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var entity = this.dbSet.Find(id);
        if (entity != null)
        {
            this.dbSet.Remove(entity);
            this.context.SaveChanges();
        }
    }

    public IEnumerable<OrderState> GetAll()
    {
        return this.dbSet.ToList();
    }

    public IEnumerable<OrderState> GetAll(int pageNumber, int rowCount)
    {
        return this.dbSet.Skip((pageNumber - 1) * rowCount).Take(rowCount).ToList();
    }

    public OrderState GetById(int id)
    {
        return this.dbSet.Find(id);
    }

    public void Update(OrderState entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        this.dbSet.Update(entity);
        this.context.SaveChanges();
    }
}