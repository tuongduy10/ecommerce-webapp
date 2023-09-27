using ECommerce.Application.BaseServices.Category.Dtos;
using ECommerce.Application.BaseServices.SubCategory.Dtos;
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
