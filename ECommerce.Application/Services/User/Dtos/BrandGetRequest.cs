using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.User.Dtos
{
    public class BrandGetRequest
    {
        public int shopId { get; set; }
        public BrandGetRequest()
        {
            shopId = -1;
        }
    }
}
