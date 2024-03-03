using System;
using System.Collections.Generic;

namespace ECommerce.Data.Entities
{
    public partial class ProductOptionValue
    {
        public int ProductId { get; set; }
        public int OptionValueId { get; set; }

        public virtual OptionValue OptionValue { get; set; }
        public virtual Product Product { get; set; }
    }
}
