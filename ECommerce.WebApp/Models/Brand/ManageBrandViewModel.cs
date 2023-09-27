using ECommerce.Application.BaseServices.Brand.Dtos;
using ECommerce.Application.BaseServices.Category.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models.Brand
{
    public class ManageBrandViewModel
    {
        public List<BrandModel> listBrand { get; set; }
        public List<CategoryModel> listCategory { get; set; }
    }
}
