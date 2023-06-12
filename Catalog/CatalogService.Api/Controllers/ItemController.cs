using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Domain;
using CatalogService.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogService.API.Controllers
{
    [Route("api/v1/items")]
    [ApiController]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private readonly IItemService itemService;
        private readonly IMessageProducer messageProducer;
        private readonly IConfiguration configuration;

        public ItemController(IItemService itemService, IMessageProducer messageProducer, IConfiguration configuration)
        {
            this.itemService = itemService;
            this.messageProducer = messageProducer;
            this.configuration = configuration;    
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
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Post([FromBody] Item item)
        {
            var result = await itemService.AddItem(item);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Put(int id, [FromBody] Item item)
        {
            var result = await itemService.UpdateItem(id, item);
            messageProducer.SendMessage(configuration["RabbitMq:ItemQueueName"], result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await itemService.DeleteItem(id);
            return Ok(result);
        }
    }
}
