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

public class OrderDetailService : ICrud
{
    private readonly StoreDAL.Repository.OrderDetailRepository repository;

    public OrderDetailService(StoreDbContext context)
    {
        this.repository = new StoreDAL.Repository.OrderDetailRepository(context);
    }

    public void Add(AbstractModel model)
    {
        var x = (OrderDetailModel)model;
        this.repository.Add(new OrderDetail(x.Id, x.OrderId, x.ProductId, x.Price, x.ProductAmount));
    }

    public void Delete(int modelId)
    {
        this.repository.DeleteById(modelId);
    }

    public IEnumerable<AbstractModel> GetAll()
    {
        return this.repository.GetAll().Select(x => new OrderDetailModel(x.Id, x.OrderId, x.ProductId, x.Price, x.ProductAmount));
    }

    public AbstractModel? GetById(int id)
    {
        var res = this.repository.GetById(id);
        if (res == null)
        {
            return null;
        }

        return new OrderDetailModel(res.Id, res.OrderId, res.ProductId, res.Price, res.ProductAmount);
    }

    public void Update(AbstractModel model)
    {
        var x = (OrderDetailModel)model;
        var existing = this.repository.GetById(x.Id);
        if (existing != null)
        {
            existing.OrderId = x.OrderId;
            existing.ProductId = x.ProductId;
            existing.Price = x.Price;
            existing.ProductAmount = x.ProductAmount;
            this.repository.Update(existing);
        }
    }
}