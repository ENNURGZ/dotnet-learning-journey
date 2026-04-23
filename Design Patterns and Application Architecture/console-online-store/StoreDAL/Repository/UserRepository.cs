using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

namespace StoreDAL.Repository
{
    public class UserRepository : AbstractRepository, IUserRepository
    {
        public UserRepository(StoreDbContext context)
            : base(context)
        {
        }

        public void Add(User entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.Password = HashPassword(entity.Password);
            this.context.Users.Add(entity);
            this.context.SaveChanges();
        }

        public void Delete(User entity)
        {
            this.context.Users.Remove(entity);
            this.context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var entity = this.context.Users.Find(id);
            if (entity != null)
            {
                this.context.Users.Remove(entity);
                this.context.SaveChanges();
            }
        }

        public IEnumerable<User> GetAll()
        {
            return this.context.Users.ToList();
        }

        public IEnumerable<User> GetAll(int pageNumber, int rowCount)
        {
            return this.context.Users.Skip((pageNumber - 1) * rowCount).Take(rowCount).ToList();
        }

        public User GetById(int id)
        {
            return this.context.Users.Find(id);
        }

        public void Update(User entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.Password = HashPassword(entity.Password);
            this.context.Users.Update(entity);
            this.context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return password;
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = System.Security.Cryptography.SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
