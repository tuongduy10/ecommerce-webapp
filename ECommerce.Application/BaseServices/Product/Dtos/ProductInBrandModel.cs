using ECommerce.Data.Models;
using System;
using System.Collections.Generic;

namespace ECommerce.Application.BaseServices.Product.Dtos
{
    public class ProductInBrandModel
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

        public decimal? PriceAvailable { get; set; }
        public decimal? DiscountAvailable { get; set; }
        public decimal? PricePreOrder { get; set; }
        public decimal? DiscountPreOrder { get; set; }


        public DateTime? ProductImportDate { get; set; }
        public string ProductImages { get; set; }
        public List<ProductPrice> Price { get; set; }
        public List<Type> Type { get; set; }
    }
}
