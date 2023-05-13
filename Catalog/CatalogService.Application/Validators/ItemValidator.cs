using CatalogService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Validators
{
    public static class ItemValidator
    {
        public static void Validate(this Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (String.IsNullOrWhiteSpace(item.Name)) 
            {
                throw new ArgumentException(nameof(item.Name));
            }
            if (item.Name.Length > 50)
            {
                throw new ArgumentException("Too long name. Max length is 50");
            }
        }
    }
}
