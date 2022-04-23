using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.FilterProduct.Dtos
{
    public class ProductOption
    {
        public int ProductId { get; set; }
        public int OptionId { get; set; }
        public string Value { get; set; }
    }
}
