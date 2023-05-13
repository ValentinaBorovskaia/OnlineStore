using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.DAL
{
    public class CartDbContext
    {
        private readonly ILiteDatabase dbContet;

        public CartDbContext(ILiteDatabase litedbContet)
        {
            dbContet = litedbContet;
        }

    }
}
