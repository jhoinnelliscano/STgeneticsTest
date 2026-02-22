using GoodHamburger.Core.Interfaces.Repositories;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Infrastructure.Persistence;

namespace GoodHamburger.Infrastructure.Repositories
{
    public class SandwichRepository(AppDbContext context) : Repository<SandwichEntity>(context), ISandwichRepository { }
}
