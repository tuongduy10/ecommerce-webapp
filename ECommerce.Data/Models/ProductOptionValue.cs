using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class ProductOptionValue
    {
        public int ProductId { get; set; }
        public int OptionValueId { get; set; }

        public virtual OptionValue OptionValue { get; set; }
        public virtual Product Product { get; set; }
    }
}
