namespace SportsStore.Models.ViewModels;

// NEW: Add ProductsListViewModel for pagination and category filtering
public class ProductsListViewModel
{
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    public PagingInfo PagingInfo { get; set; } = new();
    
    // NEW: Add CurrentCategory property for category filtering
    public string? CurrentCategory { get; set; }
}
