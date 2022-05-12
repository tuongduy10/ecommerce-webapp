using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Shop.Dtos
{
    public class ShopDetailModel
    {
        // Shop
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopPhoneNumber { get; set; }
        public string ShopMail { get; set; }
        public string ShopAddress { get; set; }
        public string ShopWardCode { get; set; }
        public string ShopDistrictCode { get; set; }
        public string ShopCityCode { get; set; }
        public DateTime ShopJoinDate { get; set; }
        public byte Tax { get; set; }
        public byte Status { get; set; }

        // Shop's bank account
        public ShopBank ShopBank { get; set; }

        // Shop's owner
        public User User { get; set; }

        // Shop's brand management
        public List<Brand> Brands { get; set; }
    }
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
    }
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime UserJoinDate { get; set; }
        public string UserPhone { get; set; }
        public string UserMail { get; set; }
        public string UserAddress { get; set; }
        public string UserWardCode { get; set; }
        public string UserDistrictCode { get; set; }
        public string UserCityCode { get; set; }
    }
    public class ShopBank
    {
        public string ShopBankName { get; set; }
        public string ShopAccountName { get; set; }
        public string ShopAccountNumber { get; set; }
    }
}
