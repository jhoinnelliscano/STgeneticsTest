using GoodHamburger.Core.Interfaces.Repositories;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Repositories
{
    public class OrderRepository(AppDbContext context) : Repository<OrderEntity>(context), IOrderRepository
    {
        public async Task<IEnumerable<OrderEntity>> GetAllWithDetailsAsync()
        {
            return await _context.Orders
                .Include(o => o.Sandwich)
                .Include(o => o.Details)
                .ThenInclude(d => d.Extra)
                .Where(o => !o.IsDeleted)
                .ToListAsync();
        }

        public async Task<OrderEntity?> GetWithDetailsByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Sandwich)
                .Include(o => o.Details)
                .ThenInclude(d => d.Extra)
                .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
        }
    }
}
