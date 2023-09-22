using ECommerce.Application.BaseServices.Product.Models;
using ECommerce.Application.BaseServices.SubCategory.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models.SizeGuide
{
    public class SizeGuideListViewModel
    {
        public List<SizeGuideModel> sizeGuides { get; set; }
        public List<SubCategoryModel> subCategories { get; set; }
    }
}
