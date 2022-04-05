using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public byte? Amount { get; set; }
        public decimal? Temporary { get; set; }
        public string DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public string DiscountCode { get; set; }
        public decimal? Total { get; set; }
        public byte? Status { get; set; }
        public int? UserId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Recipient { get; set; }
        public int? DiscountValueId { get; set; }

        public virtual Discount DiscountValueNavigation { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
