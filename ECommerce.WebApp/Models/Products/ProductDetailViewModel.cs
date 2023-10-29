
using ECommerce.Application.BaseServices.Brand.Dtos;
using ECommerce.Application.BaseServices.Discount.Dtos;
using ECommerce.Application.BaseServices.Product.Dtos;
using ECommerce.Application.BaseServices.Product.Models;
using ECommerce.Application.BaseServices.Rate.Dtos;
using ECommerce.Application.BaseServices.Shop.Dtos;
using ECommerce.Application.BaseServices.SubCategory.Dtos;
using System.Collections.Generic;

namespace ECommerce.WebApp.Models.Products
{
    public class ProductDetailViewModel
    {
        public string phone { get; set; }
        public ProductDetailModel product { get; set; }
        public List<RateGetModel> rates { get; set; }
        //public List<ProductModel> suggestion { get; set; }
        public List<Option> options { get; set; }
        public DiscountGetModel discount { get; set; }
        public List<ShopGetModel> shops { get; set; }
        public List<BrandModel> brands { get; set; }
        public List<SubCategoryModel> subCategories { get; set; }
        public List<SubCategoryModel> subSizes { get; set; }
        public List<SizeGuideModel> sizeGuides { get; set; }
    }
}
