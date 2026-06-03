using System.Diagnostics.CodeAnalysis;
using Data.Entities;

namespace Data.Tests.Comparers;

internal sealed class CustomerEqualityComparer : IEqualityComparer<Customer>
{
    public bool Equals(Customer? x, Customer? y)
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
               && x.PersonId == y.PersonId
               && x.DiscountValue == y.DiscountValue;
    }

    public int GetHashCode([DisallowNull] Customer obj)
    {
        return obj.GetHashCode();
    }
}
