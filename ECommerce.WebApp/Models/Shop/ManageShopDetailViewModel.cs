using ECommerce.Application.Services.Brand.Dtos;
using ECommerce.Application.Services.Shop.Dtos;
using ECommerce.Application.Services.User.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models.Shop
{
    public class ManageShopDetailViewModel
    {
        public UserGetModel User { get; set; }
        public ShopDetailManagedModel Shop { get; set; }
        public List<BrandModel> Brands { get; set; }
    }
}
