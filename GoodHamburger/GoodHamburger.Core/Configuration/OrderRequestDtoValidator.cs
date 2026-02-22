using FluentValidation;
using GoodHamburger.Core.DTOs;

namespace GoodHamburger.Core.Configuration
{
    public class OrderRequestDtoValidator : AbstractValidator<OrderRequestDto>
    {
        public OrderRequestDtoValidator()
        {
            RuleFor(x => x.ExtraIds)
                .Must(extraIds => extraIds == null || extraIds.GroupBy(id => id).All(g => g.Count() == 1))
                .WithMessage("Duplicate items are not allowed. Only one serving of each extra is allowed.");
        }
    }
}
