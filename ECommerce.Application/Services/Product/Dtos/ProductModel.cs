using System;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public byte? DiscountPercent { get; set; }
        public byte? Status { get; set; }
        public bool? New { get; set; } // Mới
        public bool? Highlights { get; set; } // Hot
        public int SubCategoryId { get; set; }
        public string ShopName { get; set; }
        public string BrandName { get; set; }
        public DateTime? ProductImportDate { get; set; }
        public string ProductImages { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceOnSell { get; set; }
        public string ProductTypeName { get; set; }
    }
}
