namespace SportsStore.Models.ViewModels;

// NEW: Add PagingInfo model for pagination
public class PagingInfo
{
    public int TotalItems { get; init; }
    public int ItemsPerPage { get; init; }
    public int CurrentPage { get; init; }
    public int TotalPages => this.ItemsPerPage == 0 ? 0 : (int)Math.Ceiling((decimal)this.TotalItems / this.ItemsPerPage);
}
