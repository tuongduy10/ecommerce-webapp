using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Shop.Dtos
{
    public class ShopAddRequest
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
        public string address { get; set; }
        public string wardCode { get; set; }
        public string districtCode { get; set; }
        public string cityCode { get; set; }
        public string bankName { get; set; }
        public string bankAccount { get; set; }
        public string accountNumber { get; set; }
        public int tax { get; set; }
        public List<int> brandIds { get; set; } // Shop manage Brands
    }
}
