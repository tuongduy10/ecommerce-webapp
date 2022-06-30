using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Brand.Dtos
{
    public class BrandModel
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandImagePath { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? Highlights { get; set; }
        public bool? New { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public List<ShopManage> Shops { get; set; }
    }
    public class ShopManage
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
