using ECommerce.Application.BaseServices.Brand.Dtos;
using ECommerce.Application.BaseServices.Category.Dtos;
using ECommerce.Application.BaseServices.Configurations.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models
{
    public class HomeViewModel
    {
        public List<BrandModel> listBrand { get; set; }
        public List<CategoryModel> listCategory { get; set; }
    }
}
