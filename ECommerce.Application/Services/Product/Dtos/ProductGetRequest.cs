using ECommerce.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class ProductGetRequest : PageRequest
    {
        public int? id { get; set; }
        public string keyword { get; set; }
        public int? shopId { get; set; }
        public int brandId { get; set; }    
        public int subCategoryId { get; set; }
        public int categoryId { get; set; }
        public string orderBy { get; set; } // newest | discount
        public List<int> optionValueIds { get; set; }
        public string getBy { get; set; }

        public ProductGetRequest()
        {
            keyword = "";
            id = -1;
            brandId = -1;
            subCategoryId = -1;
            categoryId = -1;
            shopId = -1;
        }
    }
}
