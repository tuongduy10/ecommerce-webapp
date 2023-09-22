using ECommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.SubCategory.Dtos
{
    public class SubCategoryModel
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public int CategoryId { get; set; }
        public SizeGuide SizeGuide { get; set; }
        public List<OptionGetModel> options { get; set; }
        public List<AttributeGetModel> attributes { get; set; }
    }
}
