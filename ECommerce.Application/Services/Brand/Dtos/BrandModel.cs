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
        public string? Description { get; set; }
        public string Category { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<ShopManage> Shops { get; set; }
    }
    public class ShopManage
    {
        public int id { get; set; }
        public string name { get; set; }
        public int status { get; set; }
    }
}
