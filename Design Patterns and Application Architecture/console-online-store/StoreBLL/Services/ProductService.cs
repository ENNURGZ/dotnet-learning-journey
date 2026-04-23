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

public class ProductService : ICrud
{
    private readonly StoreDAL.Repository.ProductRepository repository;

    public ProductService(StoreDbContext context)
    {
        this.repository = new ProductRepository(context);
    }

    public void Add(AbstractModel model)
    {
        var x = (ProductModel)model;
        this.repository.Add(new Product(x.Id, x.TitleId, x.ManufacturerId, x.Description, x.UnitPrice));
    }

    public void Delete(int modelId)
    {
        this.repository.DeleteById(modelId);
    }

    public IEnumerable<AbstractModel> GetAll()
    {
        return this.repository.GetAll().Select(x => new ProductModel(x.Id, x.TitleId, x.ManufacturerId, x.Description, x.UnitPrice));
    }

    public AbstractModel? GetById(int id)
    {
        var res = this.repository.GetById(id);
        if (res == null)
        {
            return null;
        }

        return new ProductModel(res.Id, res.TitleId, res.ManufacturerId, res.Description, res.UnitPrice);
    }

    public void Update(AbstractModel model)
    {
        var x = (ProductModel)model;
        var existing = this.repository.GetById(x.Id);
        if (existing != null)
        {
            existing.TitleId = x.TitleId;
            existing.ManufacturerId = x.ManufacturerId;
            existing.Description = x.Description;
            existing.UnitPrice = x.UnitPrice;
            this.repository.Update(existing);
        }
    }
}
