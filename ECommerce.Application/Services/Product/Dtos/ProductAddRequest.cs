using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class ProductAddRequest
    {
        public string name{ get;set; }
        public string description { get; set; }
        public string insurance { get; set; }
        public int subCategoryId { get; set; }
        public int shopId { get; set; }
        public int brandId { get; set; }
    }
}
