using ECommerce.Application.Services.SubCategory.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Product.Models
{
    public class SizeGuideModel
    {
        public int id { get; set; }
        public string content { get; set; }
        public List<SubCategoryModel> subCategories { get; set; }
    }
}
