using CartingService.BLL.Interfaces;
using CartingService.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CartingService.API.Controllers.V1
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
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
            var result = cartService.GetCartById(id);
            return Ok(result);
        }

        [ApiVersion("1.0")]
        [ApiVersion("2.0")]
        [HttpPut("{cartId}")]
        public IActionResult Put(Guid cartId, [FromBody] Item item)
        {
            cartService.AddItem(cartId, item);
            return Ok("Item added to cart");
        }

        [ApiVersion("1.0")]
        [ApiVersion("2.0")]
        [HttpDelete("{cartId}")]
        public IActionResult Delete(Guid cartId, int itemId)
        {
            cartService.RemoveItem(cartId, itemId);
            return Ok("Item Removed");

        }
    }
}
