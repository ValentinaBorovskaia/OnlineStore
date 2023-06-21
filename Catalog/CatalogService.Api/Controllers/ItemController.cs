using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Domain;
using CatalogService.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogService.API.Controllers
{
    [Route("api/v1/items")]
    [ApiController]
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

        [HttpGet("")]
        public async Task<IActionResult> Get(int? categoryId = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var categories = await itemService.GetItems(categoryId, page, pageSize);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await itemService.GetItem(id);
            return Ok(item);
        }

        [HttpGet("{id}/details")]
        public IActionResult GetDetails(int id)
        {
            var result = new Dictionary<string, string>
            {
                {"model", "the last the best" },
                {"date of release", "today" },
                {"size", "any" },
                {"colour", "your favourite" }
            };

            return Ok(result);
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
            messageProducer.SendMessage(configuration["RabbitMq:ItemQueueName"], result);
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
