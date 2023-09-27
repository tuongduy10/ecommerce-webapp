using ECommerce.Application.BaseServices.Product.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Product.Dtos
{
    public class ProductUpdateStatusRequest
    {
        public int id { get; set; }
        public int status { get; set; }
    }
}
