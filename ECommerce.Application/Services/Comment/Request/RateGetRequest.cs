using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Comment.Request
{
    public class RateGetRequest
    {
        public int id { get; set; }
        public int productId { get; set; }
        public int userId { get; set; }
        public RateGetRequest()
        {
            id = -1;
            productId = -1;
            userId = -1;
        }
    }
}
