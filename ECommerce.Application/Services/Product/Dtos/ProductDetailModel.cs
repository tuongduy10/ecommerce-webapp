using ECommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class ProductDetailModel
    {
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
        public string SubCategory { get; set; }
        public string Shop { get; set; }
        public string Brand { get; set; }
        public DateTime? ProductImportDate { get; set; }
        public byte? ProductRate { get; set; }
        public List<ProductAttribute> ProductAttributes { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductOption> ProductOptions { get; set; }
        public List<ProductPrice> ProductPrices { get; set; }
        public List<ProductUserImage> ProductUserImages { get; set; }
        public List<Rate> Rates { get; set; }
    }
}
