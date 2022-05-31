using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class Price
    {
        public decimal? price { get; set; }
        public decimal? priceOnSell { get; set; }
    }
}
