using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class OrderDeclined
    {
        public OrderDeclined()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int DeclineId { get; set; }
        public string DeclineReason { get; set; }
        public DateTime? DeclineDate { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
