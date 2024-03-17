﻿using ECommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECommerce.Application.Services.Inventory.Dtos
{
    public class CategoryModel
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public List<SubCategoryModel> subCategories { get; set; }
        public static explicit operator CategoryModel(Category data)
        {
            return new CategoryModel
            {
                categoryId = data.CategoryId,
                categoryName = data.CategoryName,
                subCategories = data.SubCategories
                    .Select(_ => new SubCategoryModel
                    {
                        id = _.SubCategoryId,
                        name = _.SubCategoryName,
                    })
                    .ToList()
            };
        }
    }
}
