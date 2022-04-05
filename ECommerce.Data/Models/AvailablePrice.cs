using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class AvailablePrice
    {
        public AvailablePrice()
        {
            Products = new HashSet<Product>();
        }

        public int AvailablePriceId { get; set; }
        public decimal? AvailablePrice1 { get; set; }
        public decimal? AvailablePriceOnSell { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
