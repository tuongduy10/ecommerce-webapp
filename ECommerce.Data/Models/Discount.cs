using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Discount
    {
        public Discount()
        {
            Brands = new HashSet<Brand>();
            Shops = new HashSet<Shop>();
        }

        public int DiscountId { get; set; }
        public decimal? DiscountValue { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountStock { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public byte? Status { get; set; }
        public bool? IsPercent { get; set; }
        public bool? ForGlobal { get; set; }
        public bool? ForShop { get; set; }
        public bool? ForBrand { get; set; }

        public virtual ICollection<Brand> Brands { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
    }
}
