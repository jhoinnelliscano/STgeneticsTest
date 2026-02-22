using GoodHamburger.Core.DTOs;
using GoodHamburger.Core.Interfaces.Repositories;
using GoodHamburger.Core.Interfaces.Services;

namespace GoodHamburger.Core.Managers
{
    public class MenuService : IMenuService
    {
        private readonly ISandwichRepository _sandwichRepository;
        private readonly IExtraRepository _extraRepository;

        public MenuService(ISandwichRepository sandwichRepository, IExtraRepository extraRepository)
        {
            _sandwichRepository = sandwichRepository;
            _extraRepository = extraRepository;
        }

        public async Task<IEnumerable<SandwichDto>> GetSandwichesAsync()
        {
            var sandwiches = await _sandwichRepository.GetAllAsync();
            return sandwiches.Select(s => new SandwichDto { Id = s.Id, Name = s.Name, Price = s.Price });
        }

        public async Task<IEnumerable<ExtraDto>> GetExtrasAsync()
        {
            var extras = await _extraRepository.GetAllAsync();
            return extras.Select(e => new ExtraDto { Id = e.Id, Name = e.Name, Price = e.Price });
        }

        public async Task<(IEnumerable<SandwichDto> Sandwiches, IEnumerable<ExtraDto> Extras)> GetFullMenuAsync()
        {
            var s = await GetSandwichesAsync();
            var e = await GetExtrasAsync();
            return (s, e);
        }
    }
}
