using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
            ShopBrands = new HashSet<ShopBrand>();
        }

        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandImagePath { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CategoryId { get; set; }
        public bool? Highlights { get; set; }
        public bool? New { get; set; }
        public int? DiscountId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Discount Discount { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ShopBrand> ShopBrands { get; set; }
    }
}
