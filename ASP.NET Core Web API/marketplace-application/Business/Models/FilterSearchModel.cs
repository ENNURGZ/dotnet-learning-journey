namespace Business.Models;

public class FilterSearchModel
{
    public int? CategoryId { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }
}
