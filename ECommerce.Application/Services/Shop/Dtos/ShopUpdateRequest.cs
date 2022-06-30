using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Shop.Dtos
{
    public class ShopUpdateRequest
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
        public string address { get; set; }
        public string wardCode { get; set; }
        public string districtCode { get; set; }
        public string cityCode { get; set; }
        public int tax { get; set; }
        public ShopBankModel shopbank { get; set; }
        public List<int> shopBrands { get; set; }
    }
}
