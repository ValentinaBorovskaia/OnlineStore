using CatalogService.Domain;
using CatalogService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Infrastructure.Interfaces
{
    public interface IItemRepository
    {
        Task<Item> Add(Item item);
        Task<Item> GetById(int id);
        Task<Item> Update(int id, Item item);
        Task<bool> Delete(int id);
        Task<IEnumerable<Item>> GetItems(int? categoryId);

    }
}
