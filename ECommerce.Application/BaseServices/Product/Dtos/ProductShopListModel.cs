using ECommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Product.Dtos
{
    public class ProductShopListModel
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string PPC { get; set; }
        public string ProductName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public byte? Status { get; set; }
        public string SubCategoryName { get; set; }
        public string CategoryName { get; set; }

        // Price
        public decimal? PriceAvailable { get; set; }
        public decimal? DiscountAvailable { get; set; }
        public decimal? PricePreOrder { get; set; }
        public decimal? DiscountPreOrder { get; set; }
        public decimal? PriceImport { get; set; }
        public decimal? PriceForSeller { get; set; }
        public decimal? ProfitAvailable { get; set; }
        public decimal? ProfitPreOrder { get; set; }
        public decimal? ProfitForSeller { get; set; }

        public string BrandName { get; set; }
        public int Stock { get; set; }
        public string ProductImages { get; set; }
        public int ShopId { get; set; }
        public int BrandId { get; set; }
        public string ShopName { get; set; }
        public string Note { get; set; }
        public string Link { get; set; }
        public List<ProductPrice> Price { get; set; }
    }
}
