using ECommerce.Application.BaseServices.Brand.Dtos;
using ECommerce.Application.BaseServices.Shop.Dtos;
using ECommerce.Application.BaseServices.SubCategory.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models.SaleProduct
{
    public class AddProductViewModel
    {
        public List<BrandModel> brands { get; set; }
        public List<ShopGetModel> shops { get; set; }
        public List<SubCategoryModel> subCategories { get; set; }
    }
}
