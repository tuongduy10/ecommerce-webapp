using ECommerce.Application.Services.Brand.Dtos;
using ECommerce.Application.Services.FilterProduct.Dtos;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Services.SubCategory.Dtos;
using System.Collections.Generic;

namespace ECommerce.WebApp.Models.Products
{
    public class ProductInBrandViewModel
    {
        public List<SubCategoryModel> listSubCategory { get; set; }
        public ProductRecordModel listProduct { get; set; }
        public BrandModel brand { get; set; }
        public List<FilterModel> listFilterModel { get; set; }
    }
}
