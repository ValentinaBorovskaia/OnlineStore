using CartingService.BLL.Interfaces;
using CartingService.DAL.Entities;
using CartingService.DAL.Interface;
using CartingService.DAL.Repositories;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.BLL.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        public bool AddItem(Guid cartId, Item item)
        {
            var existingCart = cartRepository.GetCartById(cartId);
            if (existingCart == null)
            {
                var cart = new Cart()
                {
                    Id = cartId,
                    Items = new List<Item>() { item }
                };
                cartRepository.SaveChanges(cart);
            }
            else
            {
                if (existingCart.Items.Any(x => x.Id == item.Id))
                {
                    existingCart.Items.First().Quantity += item.Quantity;
                }
                else
                {
                    existingCart.Items = existingCart.Items.Append(item);
                }
                cartRepository.SaveChanges(existingCart);
            }        
            return true;
        }

        public Cart GetCartById(Guid cartId)
        {
            return cartRepository.GetCartById(cartId);
        }

        public List<Item> GetAllItems(Guid cartId)
        {
            return cartRepository.GetCartById(cartId)?.Items.ToList();
        }

        public bool RemoveItem(Guid cartId, int itemId)
        {
            var existingCart = cartRepository.GetCartById(cartId);
            if (existingCart == null)
            {
                throw new NullReferenceException("Cart doesn't exist");
            }
            else
            {
                if (existingCart.Items.Any(x => x.Id == itemId))
                {
                    existingCart.Items = existingCart.Items.Where(x => x.Id != itemId);
                }
                else
                {
                    throw new NullReferenceException("Cart item  doesn't exist");
                }
                cartRepository.SaveChanges(existingCart);
            }
            return true;
        }

        public bool UpdateItem(Item item)
        {
             return cartRepository.UpdateItem(item);
        }
    }
}
