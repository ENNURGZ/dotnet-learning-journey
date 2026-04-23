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

public class CategoryService : ICrud
{
    private readonly StoreDAL.Repository.CategoryRepository repository;

    public CategoryService(StoreDbContext context)
    {
        this.repository = new StoreDAL.Repository.CategoryRepository(context);
    }

    public void Add(AbstractModel model)
    {
        var x = (CategoryModel)model;
        this.repository.Add(new Category(x.Id, x.CategoryName));
    }

    public void Delete(int modelId)
    {
        this.repository.DeleteById(modelId);
    }

    public IEnumerable<AbstractModel> GetAll()
    {
        return this.repository.GetAll().Select(x => new CategoryModel(x.Id, x.Name));
    }

    public AbstractModel? GetById(int id)
    {
        var res = this.repository.GetById(id);
        if (res == null)
        {
            return null;
        }

        return new CategoryModel(res.Id, res.Name);
    }

    public void Update(AbstractModel model)
    {
        var x = (CategoryModel)model;
        var existing = this.repository.GetById(x.Id);
        if (existing != null)
        {
            existing.Name = x.CategoryName;
            this.repository.Update(existing);
        }
    }
}
