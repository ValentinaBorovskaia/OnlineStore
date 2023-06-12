using CartingService.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CartingService.API.Controllers.V2
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var result = cartService.GetAllItems(id);
            return Ok(result);
        }


    }
}
