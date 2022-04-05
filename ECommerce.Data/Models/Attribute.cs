using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Attribute
    {
        public Attribute()
        {
            ProductAttributes = new HashSet<ProductAttribute>();
            SubCategoryAttributes = new HashSet<SubCategoryAttribute>();
        }

        public int AttributeId { get; set; }
        public string AttributeName { get; set; }

        public virtual ICollection<ProductAttribute> ProductAttributes { get; set; }
        public virtual ICollection<SubCategoryAttribute> SubCategoryAttributes { get; set; }
    }
}
