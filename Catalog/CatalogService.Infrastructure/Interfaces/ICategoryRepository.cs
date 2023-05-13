using CatalogService.Domain;
using CatalogService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Infrastructure.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category> Add(Category category);
        Task<Category> GetById(int id);
        Task<Category> Update(int id, Category category);
        Task<bool> Delete(int id);

    }
}
