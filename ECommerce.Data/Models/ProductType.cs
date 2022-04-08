using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class ProductType
    {
        public ProductType()
        {
            ProductPrices = new HashSet<ProductPrice>();
        }

        public int ProductTypeId { get; set; }
        public string ProductType1 { get; set; }

        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
    }
}
