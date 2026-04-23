namespace StoreBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProductTitleModel : AbstractModel
{
    public ProductTitleModel(int id, string title, int categoryId)
        : base(id)
    {
        this.Title = title;
        this.CategoryId = categoryId;
    }

    public string Title { get; set; }

    public int CategoryId { get; set; }
}
