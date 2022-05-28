using ECommerce.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class ProductGetRequest : PageRequest
    {
        public int BrandId { get; set; }
        public int SubCategoryId { get; set; }
        public string OrderBy { get; set; }
        public List<int> listOptionValueId { get; set; }
        public string GetBy { get; set; }
    }
}
