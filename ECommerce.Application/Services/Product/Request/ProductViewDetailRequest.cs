using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class ProductViewDetailRequest
    {
        public int productId { get; set; }
        public int shopId { get; set; }
        //public int subCategoryId { get; set; }
        public int brandId { get; set; }
    }
}
