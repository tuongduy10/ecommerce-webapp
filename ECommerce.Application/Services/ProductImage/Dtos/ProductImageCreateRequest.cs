using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.ProductImage.Dtos
{
    public class ProductImageCreateRequest
    {
        public string ProductImagePath { get; set; }
        public int ProductId { get; set; }
      
    }
}
