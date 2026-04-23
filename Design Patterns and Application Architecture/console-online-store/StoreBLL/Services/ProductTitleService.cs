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

public class ProductTitleService : ICrud
{
    private readonly StoreDAL.Repository.ProductTitleRepository repository;

    public ProductTitleService(StoreDbContext context)
    {
        this.repository = new StoreDAL.Repository.ProductTitleRepository(context);
    }

    public void Add(AbstractModel model)
    {
        var x = (ProductTitleModel)model;
        this.repository.Add(new ProductTitle(x.Id, x.Title, x.CategoryId));
    }

    public void Delete(int modelId)
    {
        this.repository.DeleteById(modelId);
    }

    public IEnumerable<AbstractModel> GetAll()
    {
        return this.repository.GetAll().Select(x => new ProductTitleModel(x.Id, x.Title, x.CategoryId));
    }

    public AbstractModel? GetById(int id)
    {
        var res = this.repository.GetById(id);
        if (res == null)
        {
            return null;
        }

        return new ProductTitleModel(res.Id, res.Title, res.CategoryId);
    }

    public void Update(AbstractModel model)
    {
        var x = (ProductTitleModel)model;
        var existing = this.repository.GetById(x.Id);
        if (existing != null)
        {
            existing.Title = x.Title;
            existing.CategoryId = x.CategoryId;
            this.repository.Update(existing);
        }
    }
}
