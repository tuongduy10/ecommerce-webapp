using ECommerce.Application.Services.Brand.Dtos;
using ECommerce.Application.Services.Category.Dtos;
using ECommerce.Application.Services.Configurations.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models
{
    public class HomeViewModel
    {
        public List<BrandModel> listBrand { get; set; }
        public List<SubCategoryModel> listCategory { get; set; }
    }
}
