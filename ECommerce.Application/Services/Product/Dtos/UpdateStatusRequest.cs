using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class UpdateStatusRequest
    {
        public List<int> ids { get; set; }
        public int status { get; set; }
    }
}
