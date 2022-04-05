using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class ProductUserImage
    {
        public int ProductUserImageId { get; set; }
        public string ProductUserImagePath { get; set; }
        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
