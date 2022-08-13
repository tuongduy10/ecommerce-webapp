
using ECommerce.Application.Services.Brand.Dtos;
using ECommerce.Application.Services.Discount.Dtos;
using ECommerce.Application.Services.Product.Dtos;
using ECommerce.Application.Services.Product.Models;
using ECommerce.Application.Services.Rate.Dtos;
using ECommerce.Application.Services.Shop.Dtos;
using ECommerce.Application.Services.SubCategory.Dtos;
using System.Collections.Generic;

namespace ECommerce.WebApp.Models.Products
{
    public class ProductDetailViewModel
    {
        public string phone { get; set; }
        public ProductDetailModel product { get; set; }
        public List<RateGetModel> rates { get; set; }
        public List<ProductModel> suggestion { get; set; }
        public List<Option> options { get; set; }
        public DiscountGetModel discount { get; set; }
        public List<ShopGetModel> shops { get; set; }
        public List<BrandModel> brands { get; set; }
        public List<SubCategoryModel> subCategories { get; set; }
        public List<SubCategoryModel> subSizes { get; set; }
        public List<SizeGuideModel> sizeGuides { get; set; }
    }
}
