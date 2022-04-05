using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class SubCategoryAttribute
    {
        public int AttributeId { get; set; }
        public int SubCategoryId { get; set; }

        public virtual Attribute Attribute { get; set; }
        public virtual SubCategory SubCategory { get; set; }
    }
}
