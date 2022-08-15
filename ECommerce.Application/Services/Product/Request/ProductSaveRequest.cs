using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class ProductSaveRequest
    {
        // None relationship data
        public int id { get; set; }
        public string code { get; set; }
        public string ppc { get; set; }
        public int shopId { get; set; }
        public string name{ get;set; }
        public string description { get; set; }
        public string size { get; set; }
        public string note { get; set; }
        public string link { get; set; }
        public byte? discountPercent { get; set; }
        public int stock { get; set; }
        public int subCategoryId { get; set; }
        public int userId { get; set; } // get shop by userId
        public int brandId { get; set; }
        public bool isNew { get; set; } // Mới
        public bool isHighlight { get; set; } // Nổi bật
        public bool isLegit { get; set; } // Chính hãng
        public bool isFreeReturn { get; set; } // Đổi trả miễn phí
        public bool isFreeDelivery { get; set; } // Giao hàng miễn phí
        public string insurance { get; set; } // Bảo hành
        // Relationship data
        public string currentOptions { get; set; }
        public string newOptions { get; set; }
        public string attributes { get; set; }
        public List<IFormFile> systemImage { get; set; }
        public List<IFormFile> userImage { get; set; }
        public List<string> systemFileName { get; set; }
        public List<string> userFileName { get; set; }
        public string priceAvailable { get; set; }
        public string pricePreorder { get; set; }
    }
}
