using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Discount
    {
        public Discount()
        {
            Orders = new HashSet<Order>();
        }

        public int DiscountId { get; set; }
        public decimal? DiscountValue { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountStock { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public byte? Status { get; set; }
        public int? DiscountTypeId { get; set; }

        public virtual DiscountType DiscountType { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
