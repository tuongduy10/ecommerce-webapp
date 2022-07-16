using ECommerce.Application.Services.Category.Dtos;
using ECommerce.Application.Services.SubCategory.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models.Category
{
    public class CategoryDetailViewModel
    {
        public CategoryModel category { get; set; }
        public List<SubCategoryModel> subcategories { get; set; }
    }
}
