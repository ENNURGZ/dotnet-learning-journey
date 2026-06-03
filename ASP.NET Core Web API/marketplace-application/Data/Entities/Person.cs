using System;

namespace Data.Entities;

public class Person : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public DateTime BirthDate { get; set; }
}