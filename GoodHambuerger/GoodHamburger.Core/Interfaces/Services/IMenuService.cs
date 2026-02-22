using GoodHamburger.Core.DTOs;

namespace GoodHamburger.Core.Interfaces.Services
{
    public interface IMenuService
    {
        Task<IEnumerable<SandwichDto>> GetSandwichesAsync();
        Task<IEnumerable<ExtraDto>> GetExtrasAsync();
        Task<(IEnumerable<SandwichDto> Sandwiches, IEnumerable<ExtraDto> Extras)> GetFullMenuAsync();
    }
}
