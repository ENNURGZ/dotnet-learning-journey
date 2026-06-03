using System.Diagnostics.CodeAnalysis;
using Data.Entities;

namespace Data.Tests.Comparers;

internal sealed class ReceiptDetailEqualityComparer : IEqualityComparer<ReceiptDetail>
{
    public bool Equals(ReceiptDetail? x, ReceiptDetail? y)
    {
        if (x == null && y == null)
            return true;
        if (x == null || y == null)
            return false;

        return x.Id == y.Id
               && x.ReceiptId == y.ReceiptId
               && x.ProductId == y.ProductId
               && x.UnitPrice == y.UnitPrice
               && x.DiscountUnitPrice == y.DiscountUnitPrice
               && x.Quantity == y.Quantity;
    }

    public int GetHashCode([DisallowNull] ReceiptDetail obj)
    {
        return obj.GetHashCode();
    }
}
