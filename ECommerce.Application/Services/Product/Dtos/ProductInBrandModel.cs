using ECommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Product.Dtos
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
        public string Shop { get; set; }
        public string Brand { get; set; }
        public DateTime? ProductImportDate { get; set; }
        public string ProductImages { get; set; }
        public List<ProductPrice> Price { get; set; }
        public List<Type> Type { get; set; }
    }
}
