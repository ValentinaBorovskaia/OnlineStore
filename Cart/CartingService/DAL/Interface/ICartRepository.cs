using CartingService.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.DAL.Interface
{
    public interface ICartRepository
    {
        Cart GetCartById(Guid cartId);
        bool SaveChanges(Cart cart);
        bool UpdateItem(Item item);
    }
}
