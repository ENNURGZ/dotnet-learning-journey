using System.Diagnostics.CodeAnalysis;
using Data.Entities;

namespace Data.Tests.Comparers;

internal sealed class ProductEqualityComparer : IEqualityComparer<Product>
{
    public bool Equals(Product? x, Product? y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.Id == y.Id
               && x.ProductCategoryId == y.ProductCategoryId
               && x.ProductName == y.ProductName
               && x.Price == y.Price;
    }

    public int GetHashCode([DisallowNull] Product obj)
    {
        return obj.GetHashCode();
    }
}
