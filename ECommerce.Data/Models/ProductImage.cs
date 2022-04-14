using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class ProductImage
    {
        public int ProductImageId { get; set; }
        public string ProductImagePath { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
