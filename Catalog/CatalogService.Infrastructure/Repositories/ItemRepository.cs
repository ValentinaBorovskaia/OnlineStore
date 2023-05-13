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
    public class ItemRepository : IItemRepository
    {
        private readonly CatalogContext dbContext;
        public ItemRepository(CatalogContext catalogDbContext)
        {
            dbContext = catalogDbContext;
        }

        public async Task<Item> Add(Item item)
        {
            dbContext.Items.Add(item);
            await dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<bool> Delete(int id)
        {
            var itemShouldBeDeleted = await dbContext.Items
                .FirstOrDefaultAsync(a => a.Id == id);

            if (itemShouldBeDeleted == null)
            {
                throw new NullReferenceException("Item doesn't exist");
            }

            dbContext.Items.Remove(itemShouldBeDeleted);
            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<Item>> GetItems(int? categoryId)
        {
            return await dbContext
                .Items
                .Where(x => categoryId == null || x.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<Item> GetById(int id)
        {
            var result = await dbContext.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                throw new NullReferenceException("Item doesn't exist");
            }

            return result;
        }

        public async Task<Item> Update(int id, Item item)
        {
            var itemToBeUpdated = await dbContext.Items
                .FirstOrDefaultAsync(a => a.Id == id);

            if (itemToBeUpdated == null)
            {
                throw new NullReferenceException("Item doesn't exist");
            }

            itemToBeUpdated.Price = item.Price;
            itemToBeUpdated.CategoryId = item.CategoryId;
            itemToBeUpdated.Amount = item.Amount;
            itemToBeUpdated.Description = item.Description;
            itemToBeUpdated.Name = item.Name;
            itemToBeUpdated.Image = item.Image;

            dbContext.SaveChanges();

            return itemToBeUpdated;

        }
    }
}
