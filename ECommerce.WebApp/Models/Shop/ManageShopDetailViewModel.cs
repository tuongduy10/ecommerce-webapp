using ECommerce.Application.BaseServices.Brand.Dtos;
using ECommerce.Application.BaseServices.Shop.Dtos;
using ECommerce.Application.BaseServices.User.Dtos;
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
