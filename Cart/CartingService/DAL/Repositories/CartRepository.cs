using CartingService.DAL.Entities;
using CartingService.DAL.Interface;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.DAL.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ILiteDatabase liteDatabase;
        public CartRepository(ILiteDatabase liteDatabase)
        {
            this.liteDatabase = liteDatabase;
        }
        public Cart GetCartById(Guid cartId)
        {
            return liteDatabase.GetCollection<Cart>().FindOne(x => x.Id == cartId);
        }

        public bool SaveChanges(Cart cart)
        {
            try
            {
                var colletion = liteDatabase.GetCollection<Cart>();
                var existingCart = colletion.FindOne(x => x.Id == cart.Id);
                if (existingCart == null)
                {
                    colletion.Insert(cart);
                }
                else
                {
                    existingCart.Items = cart.Items;
                    colletion.Update(existingCart);
                }
                return true;
            } 
            catch
            {
                throw;
            }
        }

        public bool UpdateItem(Item updatedItem)
        {
            var carts = liteDatabase.GetCollection<Cart>();
            foreach (var cart in carts.FindAll())
            {
                var items = cart.Items.Where(a => a.Id == updatedItem.Id);

                foreach (var item in items)
                {
                    item.Name = updatedItem.Name;
                    item.Image = updatedItem.Image;
                    item.Price = updatedItem.Price;            
                }

                cart.Items = items;
                carts.Update(cart);
            }
            return true;
        }

    }
}