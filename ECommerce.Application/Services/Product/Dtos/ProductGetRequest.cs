using ECommerce.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class ProductGetRequest : PageRequest
    {
        public int? id { get; set; }
        public int brandId { get; set; }
        public int subCategoryId { get; set; }
        public string orderBy { get; set; } // newest | discount | asc | desc
        public List<int> optionValueIds { get; set; }

        public ProductGetRequest()
        {
            id = -1;
            brandId = -1;
            subCategoryId = -1;
        }
    }
}
