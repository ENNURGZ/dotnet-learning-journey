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

public class UserService : ICrud
{
    private readonly StoreDAL.Repository.UserRepository repository;

    public UserService(StoreDbContext context)
    {
        this.repository = new UserRepository(context);
    }

    public void Add(AbstractModel model)
    {
        var x = (UserModel)model;
        this.repository.Add(new User(x.Id, x.Name, x.LastName, x.Login, x.Password, x.RoleId));
    }

    public void Delete(int modelId)
    {
        this.repository.DeleteById(modelId);
    }

    public IEnumerable<AbstractModel> GetAll()
    {
        return this.repository.GetAll().Select(x => new UserModel(x.Id, x.Name, x.LastName, x.Login, x.Password, x.RoleId));
    }

    public AbstractModel? GetById(int id)
    {
        var res = this.repository.GetById(id);
        if (res == null)
        {
            return null;
        }

        return new UserModel(res.Id, res.Name, res.LastName, res.Login, res.Password, res.RoleId);
    }

    public void Update(AbstractModel model)
    {
        var x = (UserModel)model;
        var existing = this.repository.GetById(x.Id);
        if (existing != null)
        {
            existing.Name = x.Name;
            existing.LastName = x.LastName;
            existing.Login = x.Login;
            existing.Password = x.Password;
            existing.RoleId = x.RoleId;
            this.repository.Update(existing);
        }
    }
}
