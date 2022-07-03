using ECommerce.Application.Services.Brand.Dtos;
using ECommerce.Application.Services.Shop.Dtos;
using ECommerce.Application.Services.SubCategory.Dtos;
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
    }
}
