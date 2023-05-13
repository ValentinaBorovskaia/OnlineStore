using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.DAL.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }

        public IEnumerable<Item> Items { get; set; }
    }
}
