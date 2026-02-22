using GoodHamburger.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFullMenu()
        {
            var menu = await _menuService.GetFullMenuAsync();
            return Ok(new { sandwiches = menu.Sandwiches, extras = menu.Extras });
        }

        [HttpGet("sandwiches")]
        public async Task<IActionResult> GetSandwiches()
        {
            return Ok(await _menuService.GetSandwichesAsync());
        }

        [HttpGet("extras")]
        public async Task<IActionResult> GetExtras()
        {
            return Ok(await _menuService.GetExtrasAsync());
        }
    }
}
