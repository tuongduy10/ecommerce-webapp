using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ShopId { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string AttributeValue { get; set; }
        public string OptionValue { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceOnSell { get; set; }
        public int? Amount { get; set; }
        public decimal? Total { get; set; }
        public byte? Status { get; set; }
        public bool? Paid { get; set; }
        public decimal? PayForAdmin { get; set; }

        public virtual Order Order { get; set; }
        public virtual Shop Shop { get; set; }
    }
}
