namespace StoreBLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreBLL.Interfaces;
using StoreBLL.Models;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;
using StoreDAL.Repository;

public class OrderStateService : ICrud
{
    private readonly StoreDAL.Repository.OrderStateRepository repository;

    public OrderStateService(StoreDbContext context)
    {
        this.repository = new StoreDAL.Repository.OrderStateRepository(context);
    }

    public void Add(AbstractModel model)
    {
        var x = (OrderStateModel)model;
        this.repository.Add(new OrderState(x.Id, x.StateName));
    }

    public void Delete(int modelId)
    {
        this.repository.DeleteById(modelId);
    }

    public IEnumerable<AbstractModel> GetAll()
    {
        return this.repository.GetAll().Select(x => new OrderStateModel(x.Id, x.StateName));
    }

    public AbstractModel? GetById(int id)
    {
        var res = this.repository.GetById(id);
        if (res == null)
        {
            return null;
        }

        return new OrderStateModel(res.Id, res.StateName);
    }

    public void Update(AbstractModel model)
    {
        var x = (OrderStateModel)model;
        var existing = this.repository.GetById(x.Id);
        if (existing != null)
        {
            existing.StateName = x.StateName;
            this.repository.Update(existing);
        }
    }
}