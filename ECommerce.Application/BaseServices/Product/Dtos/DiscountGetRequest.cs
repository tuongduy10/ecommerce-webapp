using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class DiscountGetRequest
    {
        public int shopId { get; set; }
        public int brandId { get; set; }
        public DiscountGetRequest()
        {
            shopId = -1;
            brandId = -1;
        }
    }
}
