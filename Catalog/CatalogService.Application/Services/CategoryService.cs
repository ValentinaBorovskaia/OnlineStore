

using CatalogService.Application.Interfaces;
using CatalogService.Application.Validators;
using CatalogService.Domain.Models;
using CatalogService.Infrastructure.Interfaces;

namespace CatalogService.Application.Services
{
    public class CategoryService : ICategoryService 
    { 
        
        private readonly ICategoryRepository categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<Category> AddCategory(Category category)
        {
            category.Validate();
            return await categoryRepository.Add(category);
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            return await categoryRepository.Delete(categoryId);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await categoryRepository.GetAll();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await categoryRepository.GetById(id);
        }

        public async Task<Category> UpdateCategory(int id, Category category)
        {
            category.Validate();
            return await categoryRepository.Update(id, category);
        }
    }
}
