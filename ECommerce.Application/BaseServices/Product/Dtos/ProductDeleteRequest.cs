using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class ProductDeleteRequest
    {
        public List<int> ids { get; set; }
    }
}
