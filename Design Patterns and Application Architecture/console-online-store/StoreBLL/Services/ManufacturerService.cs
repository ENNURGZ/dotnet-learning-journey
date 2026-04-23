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

public class ManufacturerService : ICrud
{
    private readonly StoreDAL.Repository.ManufacturerRepository repository;

    public ManufacturerService(StoreDbContext context)
    {
        this.repository = new StoreDAL.Repository.ManufacturerRepository(context);
    }

    public void Add(AbstractModel model)
    {
        var x = (ManufacturerModel)model;
        this.repository.Add(new Manufacturer(x.Id, x.ManufacturerName));
    }

    public void Delete(int modelId)
    {
        this.repository.DeleteById(modelId);
    }

    public IEnumerable<AbstractModel> GetAll()
    {
        return this.repository.GetAll().Select(x => new ManufacturerModel(x.Id, x.Name));
    }

    public AbstractModel? GetById(int id)
    {
        var res = this.repository.GetById(id);
        if (res == null)
        {
            return null;
        }

        return new ManufacturerModel(res.Id, res.Name);
    }

    public void Update(AbstractModel model)
    {
        var x = (ManufacturerModel)model;
        var existing = this.repository.GetById(x.Id);
        if (existing != null)
        {
            existing.Name = x.ManufacturerName;
            this.repository.Update(existing);
        }
    }
}