using Data.Data;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class PersonRepository : Repository<Person>, IPersonRepository
{
    public PersonRepository(TradeMarketDbContext context)
        : base(context)
    {
    }
}