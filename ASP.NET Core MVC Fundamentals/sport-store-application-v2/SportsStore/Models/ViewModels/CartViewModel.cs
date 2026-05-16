namespace SportsStore.Models.ViewModels;

// NEW: Add CartViewModel class for cart view data
public class CartViewModel
{
    public Cart Cart { get; set; } = new();
    public Uri ReturnUrl { get; set; } = new Uri("/", UriKind.Relative);
}
