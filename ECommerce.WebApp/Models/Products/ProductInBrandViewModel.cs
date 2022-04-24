using ECommerce.Application.Dtos;
using ECommerce.Application.Services.Brand.Dtos;
using ECommerce.Application.Services.FilterProduct.Dtos;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Services.SubCategory.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models.Products
{
    public class ProductInBrandViewModel
    {
        public List<SubCategoryModel> listSubCategory { get; set; }
        public PageResult<ProductInBrandModel> listProduct { get; set; }
        public BrandModel brand { get; set; }
        public List<FilterModel> listFilterModel { get; set; }
    }
}
