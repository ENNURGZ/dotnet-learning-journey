using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

namespace StoreDAL.Repository
{
    public class OrderDetailRepository : AbstractRepository, IOrderDetailRepository
    {
        public OrderDetailRepository(StoreDbContext context)
            : base(context)
        {
        }

        public void Add(OrderDetail entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            this.context.OrderDetails.Add(entity);
            this.context.SaveChanges();
        }

        public void Delete(OrderDetail entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            this.context.OrderDetails.Remove(entity);
            this.context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var entity = this.context.OrderDetails.Find(id);
            if (entity != null)
            {
                this.context.OrderDetails.Remove(entity);
                this.context.SaveChanges();
            }
        }

        public IEnumerable<OrderDetail> GetAll()
        {
            return this.context.OrderDetails.ToList();
        }

        public IEnumerable<OrderDetail> GetAll(int pageNumber, int rowCount)
        {
            return this.context.OrderDetails.Skip((pageNumber - 1) * rowCount).Take(rowCount).ToList();
        }

        public OrderDetail GetById(int id)
        {
            return this.context.OrderDetails.Find(id);
        }

        public void Update(OrderDetail entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            this.context.OrderDetails.Update(entity);
            this.context.SaveChanges();
        }
    }
}
