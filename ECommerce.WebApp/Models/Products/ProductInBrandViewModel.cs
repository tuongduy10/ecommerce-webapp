using ECommerce.Application.BaseServices.Brand.Dtos;
using ECommerce.Application.BaseServices.FilterProduct.Dtos;
using ECommerce.Application.BaseServices.Product.Dtos;
using ECommerce.Application.BaseServices.SubCategory.Dtos;
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
