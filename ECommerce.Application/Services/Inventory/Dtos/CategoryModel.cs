using ECommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Inventory.Dtos
{
    public class CategoryModel
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public static explicit operator CategoryModel(Category data)
        {
            return new CategoryModel
            {
                categoryId = data.CategoryId,
                categoryName = data.CategoryName,
            };
        }
    }
}
