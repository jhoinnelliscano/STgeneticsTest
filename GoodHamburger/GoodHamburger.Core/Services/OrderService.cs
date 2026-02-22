using AutoMapper;
using FluentValidation;
using GoodHamburger.Core.DTOs;
using GoodHamburger.Core.Interfaces.Repositories;
using GoodHamburger.Core.Interfaces.Services;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Exception;

namespace GoodHamburger.Core.Managers
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISandwichRepository _sandwichRepository;
        private readonly IExtraRepository _extraRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<OrderRequestDto> _orderRequestDtoValidator;

        public OrderService(
            IOrderRepository orderRepository,
            ISandwichRepository sandwichRepository,
            IExtraRepository extraRepository,
            IMapper mapper,
            IValidator<OrderRequestDto> orderRequestDtoValidator)
        {
            _orderRepository = orderRepository;
            _sandwichRepository = sandwichRepository;
            _extraRepository = extraRepository;
            _mapper = mapper;
            _orderRequestDtoValidator = orderRequestDtoValidator;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(OrderRequestDto request)
        {
            await _orderRequestDtoValidator.ValidateAndThrowAsync(request);

            var order = await MapAndValidateRequest(request);
            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return MapToResponse(order);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllWithDetailsAsync();
            return orders.Select(MapToResponse);
        }

        public async Task<OrderResponseDto> UpdateOrderAsync(int id, OrderRequestDto request)
        {
            var existingOrder = await _orderRepository.GetWithDetailsByIdAsync(id);
            if (existingOrder == null) throw new DomainException("Order not found.");

            var updatedOrder = await MapAndValidateRequest(request);

            existingOrder.IdSandwich = updatedOrder.IdSandwich;
            existingOrder.Details = updatedOrder.Details;
            existingOrder.TotalBeforeDiscount = updatedOrder.TotalBeforeDiscount;
            existingOrder.Discount = updatedOrder.Discount;
            existingOrder.Total = updatedOrder.Total;
            existingOrder.UpdatedAt = DateTime.UtcNow;

            _orderRepository.Update(existingOrder);
            await _orderRepository.SaveChangesAsync();

            return MapToResponse(existingOrder);
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) throw new DomainException("Order not found.");

            order.IsDeleted = true;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }

        private async Task<OrderEntity> MapAndValidateRequest(OrderRequestDto request)
        {
            var sandwich = await _sandwichRepository.GetByIdAsync(request.SandwichId);
            if (sandwich == null) throw new DomainException("Invalid Sandwich selected.");

            var extras = new List<ExtraEntity>();
            foreach (var extraId in request.ExtraIds)
            {
                var extra = await _extraRepository.GetByIdAsync(extraId);
                if (extra == null) throw new DomainException($"Extra with ID {extraId} not found.");

                extras.Add(extra);
            }

            if (extras.Count(e => e.Name.Contains("Fries", StringComparison.OrdinalIgnoreCase)) > 1)
                throw new DomainException("Only one serving of Fries allowed.");

            if (extras.Count(e => e.Name.Contains("Soft drink", StringComparison.OrdinalIgnoreCase)) > 1)
                throw new DomainException("Only one serving of Soft drink allowed.");

            var order = _mapper.Map<OrderEntity>(request);
            order.Sandwich = sandwich;
            order.CreatedAt = DateTime.UtcNow;
            order.Details = extras.Select(e => new OrderDetailEntity
            {
                IdExtra = e.Id,
                Extra = e,
                Quantity = 1,
                Total = e.Price
            }).ToList();
            order.CalculateTotals();

            return order;
        }

        private OrderResponseDto MapToResponse(OrderEntity order)
        {
            return _mapper.Map<OrderResponseDto>(order);
        }
    }
}
