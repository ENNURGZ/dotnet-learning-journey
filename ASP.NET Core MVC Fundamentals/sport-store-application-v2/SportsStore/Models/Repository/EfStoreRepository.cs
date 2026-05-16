namespace SportsStore.Models.Repository;

public class EfStoreRepository(StoreDbContext ctx) : IStoreRepository
{
    private readonly StoreDbContext context = ctx ?? throw new ArgumentNullException(nameof(ctx));

    public IQueryable<Product> Products => this.context.Products;

    public void CreateProduct(Product p)
    {
        this.context.Add(p);
        this.context.SaveChanges();
    }

    public void DeleteProduct(Product p)
    {
        this.context.Remove(p);
        this.context.SaveChanges();
    }

    public void SaveProduct(Product p)
    {
        if (p.ProductId == 0)
        {
            this.context.Products.Add(p);
        }
        else
        {
            Product? dbEntry = this.context.Products?.FirstOrDefault(pEntry => pEntry.ProductId == p.ProductId);
            if (dbEntry != null)
            {
                dbEntry.Name = p.Name;
                dbEntry.Description = p.Description;
                dbEntry.Price = p.Price;
                dbEntry.Category = p.Category;
            }
        }
        this.context.SaveChanges();
    }
}
