using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Shop.Dtos
{
    public class ShopDetailManagedModel
    {
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopPhoneNumber { get; set; }
        public string ShopMail { get; set; }
        public string ShopAddress { get; set; }
        public string ShopWardCode { get; set; }
        public string ShopDistrictCode { get; set; }
        public string ShopCityCode { get; set; }
        public DateTime ShopJoinDate { get; set; }
        public byte? Tax { get; set; }
        public byte Status { get; set; }
        public List<int> ShopBrands { get; set; }
        public ShopBankModel ShopBank { get; set; }
    }
}
