using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ProductAttributes = new HashSet<ProductAttribute>();
            ProductImages = new HashSet<ProductImage>();
            ProductOptions = new HashSet<ProductOption>();
            ProductUserImages = new HashSet<ProductUserImage>();
            Rates = new HashSet<Rate>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public byte? DiscountPercent { get; set; }
        public DateTime? ProductAddedDate { get; set; }
        public string ProductDescription { get; set; }
        public int? ProductStock { get; set; }
        public byte? Status { get; set; }
        public bool? New { get; set; }
        public bool? Highlights { get; set; }
        public bool? FreeDelivery { get; set; }
        public bool? FreeReturn { get; set; }
        public bool? Legit { get; set; }
        public string Insurance { get; set; }
        public int? OrderPriceId { get; set; }
        public int? AvailablePriceId { get; set; }
        public int SubCategoryId { get; set; }
        public int ShopId { get; set; }
        public int BrandId { get; set; }
        public DateTime? ProductImportDate { get; set; }
        public byte? ProductRate { get; set; }

        public virtual AvailablePrice AvailablePrice { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual OrderPrice OrderPrice { get; set; }
        public virtual Shop Shop { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductAttribute> ProductAttributes { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductOption> ProductOptions { get; set; }
        public virtual ICollection<ProductUserImage> ProductUserImages { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
    }
}
