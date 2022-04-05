using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class OrderPrice
    {
        public OrderPrice()
        {
            Products = new HashSet<Product>();
        }

        public int OrderPriceId { get; set; }
        public decimal? OrderPrice1 { get; set; }
        public decimal? OrderPriceOnSell { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
