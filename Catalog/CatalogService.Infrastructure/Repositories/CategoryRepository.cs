using CatalogService.Domain;
using CatalogService.Domain.Models;
using CatalogService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogContext dbContext;
        public CategoryRepository(CatalogContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Category> Add(Category category)
        {
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<bool> Delete(int id)
        {
            var category = dbContext.Categories.Find(id);

            if (category == null)
            {
                throw new NullReferenceException("Category doesn't exist");
            }

            var items = dbContext.Items
                .Where(a => a.CategoryId == id);

            dbContext.Items.RemoveRange(items);
            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            var result = await dbContext.Categories
                .FirstOrDefaultAsync(category => category.Id == id);

            if (result == null)
            {
                throw new NullReferenceException("Category doesn't exist");
            }

            return result;
        }

        public async Task<Category> Update(int id, Category category)
        {
            var record = dbContext.Categories.Find(id);

            if (record == null)
            {
                throw new NullReferenceException("Category doesn't exist");
            }

            record.Name = category.Name;
            record.Image = category.Image;
            record.ParentId = category.ParentId;

            dbContext.Update(record);
            await dbContext.SaveChangesAsync();

            return record;
        }
    }
}
