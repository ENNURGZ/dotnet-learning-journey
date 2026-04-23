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
public class CustomerOrderService : ICrud
{
    private readonly StoreDAL.Repository.CustomerOrderRepository repository;

    public CustomerOrderService(StoreDbContext context)
    {
        this.repository = new StoreDAL.Repository.CustomerOrderRepository(context);
    }

    public void Add(AbstractModel model)
    {
        var x = (CustomerOrderModel)model;
        this.repository.Add(new CustomerOrder(x.Id, x.OperationTime, x.UserId, x.OrderStateId));
    }

    public void Delete(int modelId)
    {
        this.repository.DeleteById(modelId);
    }

    public IEnumerable<AbstractModel> GetAll()
    {
        return this.repository.GetAll().Select(x => new CustomerOrderModel(x.Id, x.OperationTime, x.UserId, x.OrderStateId));
    }

    public AbstractModel? GetById(int id)
    {
        var res = this.repository.GetById(id);
        if (res == null)
        {
            return null;
        }

        return new CustomerOrderModel(res.Id, res.OperationTime, res.UserId, res.OrderStateId);
    }

    public void Update(AbstractModel model)
    {
        var x = (CustomerOrderModel)model;
        var existing = this.repository.GetById(x.Id);
        if (existing != null)
        {
            existing.OperationTime = x.OperationTime;
            existing.UserId = x.UserId;
            existing.OrderStateId = x.OrderStateId;
            this.repository.Update(existing);
        }
    }
}