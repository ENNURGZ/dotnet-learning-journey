using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

namespace StoreDAL.Repository
{
    public class CustomerOrderRepository : AbstractRepository, ICustomerOrderRepository
    {
        public CustomerOrderRepository(StoreDbContext context)
            : base(context)
        {
        }

        public void Add(CustomerOrder entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            this.context.CustomerOrders.Add(entity);
            this.context.SaveChanges();
        }

        public void Delete(CustomerOrder entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            this.context.CustomerOrders.Remove(entity);
            this.context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var entity = this.context.CustomerOrders.Find(id);
            if (entity != null)
            {
                this.context.CustomerOrders.Remove(entity);
                this.context.SaveChanges();
            }
        }

        public IEnumerable<CustomerOrder> GetAll()
        {
            return this.context.CustomerOrders.ToList();
        }

        public IEnumerable<CustomerOrder> GetAll(int pageNumber, int rowCount)
        {
            return this.context.CustomerOrders.Skip((pageNumber - 1) * rowCount).Take(rowCount).ToList();
        }

        public CustomerOrder GetById(int id)
        {
            return this.context.CustomerOrders.Find(id);
        }

        public void Update(CustomerOrder entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            this.context.CustomerOrders.Update(entity);
            this.context.SaveChanges();
        }
    }
}
