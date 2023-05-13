using CatalogService.Domain;
using CatalogService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> AddCategory(Category category);
        Task<Category> UpdateCategory(int id, Category category);
        Task<bool> DeleteCategory(int categoryId);
        Task<Category> GetCategoryById(int id);

    }
}
