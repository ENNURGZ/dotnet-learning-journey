namespace StoreDAL.Entities;
using StoreDAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("categories")]
public class Category : BaseEntity
{
    public Category()
        : base()
    {
    }

    public Category(int id, string name)
        : base(id)
    {
        this.Name = name;
    }

    [Column("category_name")]
    public string Name { get; set; }

    public virtual IList<ProductTitle> Titles { get; set; }
}
