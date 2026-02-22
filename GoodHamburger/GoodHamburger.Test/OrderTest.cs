using AutoMapper; // Added for AutoMapper
using FluentValidation; // Added for FluentValidation
using GoodHamburger.Core.DTOs;
using GoodHamburger.Core.Interfaces.Repositories;
using GoodHamburger.Core.Managers;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Exception;
using Moq;
using Xunit;

namespace GoodHamburger.Test
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepoMock;
        private readonly Mock<ISandwichRepository> _sandwichRepoMock;
        private readonly Mock<IExtraRepository> _extraRepoMock;
        private readonly Mock<IMapper> _mapperMock; // Added IMapper mock
        private readonly Mock<IValidator<OrderRequestDto>> _orderRequestDtoValidatorMock; // Added IValidator mock
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepoMock = new Mock<IOrderRepository>();
            _sandwichRepoMock = new Mock<ISandwichRepository>();
            _extraRepoMock = new Mock<IExtraRepository>();
            _mapperMock = new Mock<IMapper>(); // Initialize IMapper mock
            _orderRequestDtoValidatorMock = new Mock<IValidator<OrderRequestDto>>(); // Initialize IValidator mock

            // Setup AutoMapper mocks for the specific mappings used in OrderService
            _mapperMock.Setup(m => m.Map<OrderEntity>(It.IsAny<OrderRequestDto>()))
                       .Returns((OrderRequestDto src) => new OrderEntity { IdSandwich = src.SandwichId, CreatedAt = DateTime.UtcNow });
            _mapperMock.Setup(m => m.Map<OrderResponseDto>(It.IsAny<OrderEntity>()))
                       .Returns((OrderEntity src) => new OrderResponseDto
                       {
                           Id = src.Id,
                           SandwichName = src.Sandwich.Name,
                           Extras = src.Details.Select(d => d.Extra.Name).ToList(),
                           TotalBeforeDiscount = src.TotalBeforeDiscount,
                           Discount = src.Discount,
                           Total = src.Total
                       });
            
            // Setup FluentValidation mock to always return success for now
            _orderRequestDtoValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<OrderRequestDto>(), It.IsAny<CancellationToken>()))
                                         .ReturnsAsync(new FluentValidation.Results.ValidationResult());


            _orderService = new OrderService(_orderRepoMock.Object, _sandwichRepoMock.Object, _extraRepoMock.Object, _mapperMock.Object, _orderRequestDtoValidatorMock.Object);
        }

        [Fact]
        public async Task CreateOrder_WithSandwichFriesAndDrink_Applies20PercentDiscount()
        {
            // Arrange
            var sandwich = new SandwichEntity { Id = 1, Name = "Burger", Price = 5.00m };
            var fries = new ExtraEntity { Id = 1, Name = "Fries", Price = 2.00m };
            var drink = new ExtraEntity { Id = 2, Name = "Soft drink", Price = 2.50m };

            _sandwichRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(sandwich);
            _extraRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fries);
            _extraRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(drink);

            var request = new OrderRequestDto
            {
                SandwichId = 1,
                ExtraIds = new List<int> { 1, 2 }
            };

            // Act
            var result = await _orderService.CreateOrderAsync(request);

            // Assert
            // Total: 5 + 2 + 2.5 = 9.5. Discount 20% = 1.9. Final = 7.6
            Assert.Equal(20, result.Discount);
            Assert.Equal(7.60m, result.Total);
        }

        [Fact]
        public async Task CreateOrder_WithDuplicateExtra_ThrowsDomainException()
        {
            // Arrange
            var sandwich = new SandwichEntity { Id = 1, Name = "Burger", Price = 5.00m };
            var fries = new ExtraEntity { Id = 1, Name = "Fries", Price = 2.00m };

            _sandwichRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(sandwich);
            _extraRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fries);

            var request = new OrderRequestDto
            {
                SandwichId = 1,
                ExtraIds = new List<int> { 1, 1 } // Duplicate Fries
            };

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(() => _orderService.CreateOrderAsync(request));
        }
    }
}
