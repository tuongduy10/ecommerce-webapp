using ECommerce.Application.BaseServices.SubCategory.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models.SizeGuide
{
    public class SizeGuideDetailViewModel
    {
        public Data.Models.SizeGuide SizeGuide { get; set; }
        public List<SubCategoryModel> SubCategories { get; set; }
    }
}
