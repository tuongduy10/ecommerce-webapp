using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class ProductOption
    {
        public int ProductId { get; set; }
        public int OptionId { get; set; }
        public string Value { get; set; }

        public virtual Option Product { get; set; }
        public virtual Product ProductNavigation { get; set; }
    }
}
