using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Shop.Dtos
{
    public class SaleRegistrationRequest
    {
        public string ShopName { get; set; }
        public string ShopPhoneNumber { get; set; }
        public string ShopMail { get; set; }
        public string ShopAddress { get; set; }
        public string ShopWardCode { get; set; }
        public string ShopDistrictCode { get; set; }
        public string ShopCityCode { get; set; }
        public string ShopBankName { get; set; }
        public string ShopAccountNumber { get; set; }
        public string ShopAccountName { get; set; }
        public int UserId { get; set; }
    }
}
