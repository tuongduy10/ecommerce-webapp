using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class OptionValue
    {
        public OptionValue()
        {
            ProductOptionValues = new HashSet<ProductOptionValue>();
        }

        public int OptionValueId { get; set; }
        public string OptionValueName { get; set; }
        public int OptionId { get; set; }

        public virtual Option Option { get; set; }
        public virtual ICollection<ProductOptionValue> ProductOptionValues { get; set; }
    }
}
