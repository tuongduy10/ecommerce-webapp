using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Brand
    {
        public Brand()
        {
            BrandCategories = new HashSet<BrandCategory>();
            Products = new HashSet<Product>();
            ShopBrands = new HashSet<ShopBrand>();
        }

        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandImagePath { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? Highlights { get; set; }
        public bool? New { get; set; }
        public int? DiscountId { get; set; }
        public string? Description { get; set; }

        public virtual Discount Discount { get; set; }
        public virtual ICollection<BrandCategory> BrandCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ShopBrand> ShopBrands { get; set; }
    }
}
