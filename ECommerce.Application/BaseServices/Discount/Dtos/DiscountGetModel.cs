using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Discount.Dtos
{
    public class DiscountGetModel
    {
        public int DiscountId { get; set; }
        public decimal? DiscountValue { get; set; }
        public string DiscountCode { get; set; }
        public bool? IsPercent { get; set; }
    }
}
