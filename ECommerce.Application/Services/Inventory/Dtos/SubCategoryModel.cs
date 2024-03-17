using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Inventory.Dtos
{
    public class SubCategoryModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int categoryId { get; set; }
        public CategoryModel category { get; set; }
        public SizeGuideModel sizeGuide { get; set; }
        public List<OptionModel> optionList { get; set; }
        public List<AttributeModel> attributes { get; set; }
    }
}
