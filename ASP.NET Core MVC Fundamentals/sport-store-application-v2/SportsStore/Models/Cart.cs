namespace SportsStore.Models;

public class Cart
{
    public List<CartLine> Lines { get; } = new();
    
    public virtual void AddItem(Product product, int quantity)
    {
        CartLine? line = Lines
            .FirstOrDefault(p => p.Product.ProductId == product.ProductId);
        if (line is null)
        {
            Lines.Add(new CartLine
            {
                Product = product,
                Quantity = quantity,
            });
        }
        else
        {
            line.Quantity += quantity;
        }
    }
    
    public virtual void RemoveLine(Product product)
        => this.RemoveLine(product.ProductId);

    public virtual void RemoveLine(long productId)
        => Lines.RemoveAll(l => l.Product.ProductId == productId);
        
    public decimal ComputeTotalValue()
        => Lines.Sum(e => e.Product.Price * e.Quantity);
        
    public virtual void Clear() => Lines.Clear();
}

public class CartLine
{
    public int CartLineId { get; set; }
    public Product Product { get; set; } = new();
    public int Quantity { get; set; }
}
