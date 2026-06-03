using System;
using System.Collections.Generic;

namespace Business.Models;

public class CustomerModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public int DiscountValue { get; set; }

    public ICollection<int>? ReceiptsIds { get; set; }
}
