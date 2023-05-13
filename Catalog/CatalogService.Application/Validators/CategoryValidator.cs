using CatalogService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Validators
{
    public static class CategoryValidator
    {
        public static void Validate(this Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException("category");
            }
            if (String.IsNullOrWhiteSpace(category.Name)) 
            {
                throw new ArgumentException("Empty name");
            }
            if(category.Name.Length > 50) 
            {
                throw new ArgumentException("Too long name. Maximum is 50 chars");
            }
        }
    }
}
