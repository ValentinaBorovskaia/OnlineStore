using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Domain;
using CatalogService.Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogService.Api.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IMessageProducer messageProducer;

        public CategoryController(ICategoryService categoryService,
            IMessageProducer messageProducer)
        {
            this.categoryService = categoryService;
            this.messageProducer = messageProducer;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await categoryService.GetCategories();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = categoryService.GetCategoryById(id);
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category category)
        {
            var result = await categoryService.AddCategory(category);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Category category)
        {
            var result = await categoryService.UpdateCategory(id, category);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await categoryService.DeleteCategory(id);
            return Ok(result);
        }
    }
}
