using System.Collections.Generic;

namespace Business.Models;

public class CategoryModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<int>? ProductIds { get; set; }
}
