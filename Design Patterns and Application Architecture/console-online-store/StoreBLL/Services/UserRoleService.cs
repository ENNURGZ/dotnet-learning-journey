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

public class UserRoleService : ICrud
{
    private readonly StoreDAL.Repository.UserRoleRepository repository;

    public UserRoleService(StoreDbContext context)
    {
        this.repository = new UserRoleRepository(context);
    }

    public void Add(AbstractModel model)
    {
        var x = (UserRoleModel)model;
        this.repository.Add(new UserRole(x.Id, x.RoleName));
    }

    public void Delete(int modelId)
    {
        this.repository.DeleteById(modelId);
    }

    public IEnumerable<AbstractModel> GetAll()
    {
        return this.repository.GetAll().Select(x => new UserRoleModel(x.Id, x.RoleName));
    }

    public AbstractModel? GetById(int id)
    {
        var res = this.repository.GetById(id);
        if (res == null)
        {
            return null;
        }

        return new UserRoleModel(res.Id, res.RoleName);
    }

    public void Update(AbstractModel model)
    {
        var x = (UserRoleModel)model;
        var existing = this.repository.GetById(x.Id);
        if (existing != null)
        {
            existing.RoleName = x.RoleName;
            this.repository.Update(existing);
        }
    }
}
