using CartingService.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CartingService.API.Controllers.V2
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpGet("{cartId}")]
        public IActionResult Get(Guid id)
        {
            var result = cartService.GetAllItems(id);
            return Ok(result);
        }


    }
}
