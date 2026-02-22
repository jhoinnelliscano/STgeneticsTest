using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Core.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<OrderEntity>
    {
        Task<IEnumerable<OrderEntity>> GetAllWithDetailsAsync();
        Task<OrderEntity?> GetWithDetailsByIdAsync(int id);
    }
}
