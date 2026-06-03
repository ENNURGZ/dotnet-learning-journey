using System.Diagnostics.CodeAnalysis;
using Data.Entities;

namespace Data.Tests.Comparers;

internal sealed class ReceiptEqualityComparer : IEqualityComparer<Receipt>
{
    public bool Equals(Receipt? x, Receipt? y)
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
               && x.CustomerId == y.CustomerId
               && x.OperationDate == y.OperationDate
               && x.IsCheckedOut == y.IsCheckedOut;
    }

    public int GetHashCode([DisallowNull] Receipt obj)
    {
        return obj.GetHashCode();
    }
}
