using CatalogService.Domain;
using CatalogService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetItems(int? categoryId, int page = 1, int pageSize = 5);
        Task<Item> GetItem(int id);
        Task<Item> UpdateItem(int id, Item item);
        Task<Item> AddItem(Item item);
        Task<bool> DeleteItem(int id);

    }
}

