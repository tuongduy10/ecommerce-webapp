using System;
using System.Collections.Generic;

namespace ECommerce.Data.Models
{
    public partial class ProductAttribute
    {
        public int ProductId { get; set; }
        public int AttributeId { get; set; }
        public string Value { get; set; }

        public virtual Attribute Attribute { get; set; }
        public virtual Product Product { get; set; }
    }
}
