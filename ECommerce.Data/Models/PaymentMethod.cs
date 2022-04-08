using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            Orders = new HashSet<Order>();
        }

        public int PaymentMethodId { get; set; }
        public string PaymentMethod1 { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
