using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Domain;
using CatalogService.Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogService.API.Controllers
{
    [Route("api/v1/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService itemService;

        public ItemController(IItemService itemService)
        {
            this.itemService = itemService;
        }

        [HttpGet("{categoryId?}")]
        public async Task<IActionResult> Get(int? categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var categories = await itemService.GetItems(categoryId, page, pageSize);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = itemService.GetItem(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Item item)
        {
            var result = await itemService.AddItem(item);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Item item)
        {
            var result = await itemService.UpdateItem(id, item);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await itemService.DeleteItem(id);
            return Ok(result);
        }
    }
}
