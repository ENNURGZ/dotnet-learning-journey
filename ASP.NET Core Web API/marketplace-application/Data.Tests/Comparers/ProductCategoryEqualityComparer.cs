using System.Diagnostics.CodeAnalysis;
using Data.Entities;

namespace Data.Tests.Comparers;

internal sealed class ProductCategoryEqualityComparer : IEqualityComparer<ProductCategory>
{
    public bool Equals(ProductCategory? x, ProductCategory? y)
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
               && x.CategoryName == y.CategoryName;
    }

    public int GetHashCode([DisallowNull] ProductCategory obj)
    {
        return obj.GetHashCode();
    }
}
