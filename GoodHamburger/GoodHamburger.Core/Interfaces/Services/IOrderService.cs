using GoodHamburger.Core.DTOs;

namespace GoodHamburger.Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(OrderRequestDto request);
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
        Task<OrderResponseDto> UpdateOrderAsync(int id, OrderRequestDto request);
        Task DeleteOrderAsync(int id);
    }
}
