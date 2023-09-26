using ECommerce.Application.Services.Inventory.Dtos;
using ECommerce.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class ProductModel
    {
        public int id { get; set; }
        public string? ppc { get; set; }
        public string? code { get; set; }
        public string name { get; set; }
        public byte? discountPercent { get; set; }
        public int? stock { get; set; }
        public byte? status { get; set; }
        public bool? isNew { get; set; } // Mới
        public bool? isHighlights { get; set; } // Hot
        public bool? isLegit { get; set; } // Hot
        // sub category
        public int subCategoryId { get; set; }
        public string? subCategoryName { get; set; }
        public string? categoryName { get; set; }
        // shop
        public int? shopId { get; set; }
        public string shopName { get; set; }
        // brand
        public int? brandId { get; set; }
        public string brandName { get; set; }

        public DateTime? createdDate { get; set; }
        public DateTime? importDate { get; set; }
        public List<string> imagePaths { get; set; }
        public List<ProductPrice> price { get; set; }
        public List<Type> type { get; set; }
        public string? description { get; set; }
        public string? size { get; set; }
        public string? link { get; set; }
        public string? note { get; set; }
        public string? repay { get; set; } // Đổi trả
        public string? delivery { get; set; } // Giao hàng
        public string? insurance { get; set; } // Bảo hành
        // Price
        public decimal? priceAvailable { get; set; }
        public decimal? discountAvailable { get; set; }
        public decimal? pricePreOrder { get; set; }
        public decimal? discountPreOrder { get; set; }
        public decimal? priceImport { get; set; }
        public decimal? priceForSeller { get; set; }
        // Profit from price
        public decimal? profitAvailable { get; set; }
        public decimal? profitPreOrder { get; set; }
        public decimal? profitForSeller { get; set; }
        // Relationship data
        public List<int>? currentOptions { get; set; }
        public List<OptionModel>? newOptions { get; set; }
        public List<string>? attributes { get; set; }
        public List<IFormFile>? systemImage { get; set; }
        public List<IFormFile>? userImage { get; set; }
        public List<string>? systemFileName { get; set; }
        public List<string>? userFileName { get; set; }
        public ProductModel()
        {
            id = -1;
            shopId = -1;
            brandId = -1;
            subCategoryId = -1;
        }
    }
}
