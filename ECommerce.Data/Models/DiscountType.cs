using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class DiscountType
    {
        public DiscountType()
        {
            Discounts = new HashSet<Discount>();
        }

        public int DiscountTypeId { get; set; }
        public string DiscountType1 { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<Discount> Discounts { get; set; }
    }
}
