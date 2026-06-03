using System.Diagnostics.CodeAnalysis;
using Data.Entities;

namespace Data.Tests.Comparers;

internal sealed class PersonEqualityComparer : IEqualityComparer<Person>
{
    public bool Equals(Person? x, Person? y)
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
               && x.Name == y.Name
               && x.Surname == y.Surname
               && x.BirthDate == y.BirthDate;
    }

    public int GetHashCode([DisallowNull] Person obj)
    {
        return obj.GetHashCode();
    }
}
