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
    public class ProductRepository : AbstractRepository, IProductRepository
    {
        public ProductRepository(StoreDbContext context)
            : base(context)
        {
        }

        public void Add(Product entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            this.context.Products.Add(entity);
            this.context.SaveChanges();
        }

        public void Delete(Product entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            this.context.Products.Remove(entity);
            this.context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var entity = this.context.Products.Find(id);
            if (entity != null)
            {
                this.context.Products.Remove(entity);
                this.context.SaveChanges();
            }
        }

        public IEnumerable<Product> GetAll()
        {
            return this.context.Products.ToList();
        }

        public IEnumerable<Product> GetAll(int pageNumber, int rowCount)
        {
            return this.context.Products.Skip((pageNumber - 1) * rowCount).Take(rowCount).ToList();
        }

        public Product GetById(int id)
        {
            return this.context.Products.Find(id);
        }

        public void Update(Product entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            this.context.Products.Update(entity);
            this.context.SaveChanges();
        }
    }
}