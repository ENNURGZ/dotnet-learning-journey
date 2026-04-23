namespace StoreBLL.Models;
using StoreDAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CategoryModel : AbstractModel
{
    public CategoryModel(int id, string name)
        : base(id)
    {
        this.CategoryName = name;
    }

    public string CategoryName { get; set; }

    public override string ToString()
    {
        return $"Id: {this.Id}, Name: {this.CategoryName}";
    }
}
