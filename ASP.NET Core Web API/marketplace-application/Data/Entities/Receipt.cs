using System;
using System.Collections.Generic;

namespace Data.Entities;

public class Receipt : BaseEntity
{
    public int CustomerId { get; set; }

    public DateTime OperationDate { get; set; }

    public bool IsCheckedOut { get; set; }

    public Customer Customer { get; set; } = null!;

    // Initialize to null to allow mapping to produce null as per tests
    public ICollection<ReceiptDetail>? ReceiptDetails { get; set; }
}