using CatalogService.Application.Interfaces;
using CatalogService.Application.Validators;
using CatalogService.Domain;
using CatalogService.Domain.Models;
using CatalogService.Infrastructure;
using CatalogService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository itemRepository;
        public ItemService(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public async Task<Item> AddItem(Item item)
        {
            item.Validate();
            return await itemRepository.Add(item);
        }

        public async Task<bool> DeleteItem(int id)
        {
            return await itemRepository.Delete(id);
        }

        public async Task<IEnumerable<Item>> GetItems(int? categoryId, int page = 1, int pageSize = 5)
        {
            return await itemRepository.GetItems(categoryId);
        }

        public async Task<Item> GetItem(int id)
        {
            return await itemRepository.GetById(id);
        }

        public async Task<Item> UpdateItem(int id, Item item)
        {
            item.Validate();
            return await itemRepository.Update(id, item);
        }

    }
}
