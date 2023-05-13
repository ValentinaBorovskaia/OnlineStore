using CartingService.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.BLL.Interfaces
{
    public interface ICartService
    {
        bool AddItem(Guid cartId, Item item);
        Cart GetCartById(Guid cartId);
        List<Item> GetAllItems(Guid cartId);
        bool RemoveItem(Guid cartId, int itemId);
        bool UpdateItem(Item item);
    }
}
