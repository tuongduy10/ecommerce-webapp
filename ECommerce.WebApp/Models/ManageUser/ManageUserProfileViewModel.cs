using ECommerce.Application.Services.Shop.Dtos;
using ECommerce.Application.Services.User.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models.ManageUser
{
    public class ManageUserProfileViewModel
    {
        public UserGetModel User { get; set; }
        public List<ShopGetModel> Shops { get; set; }
    }
}
