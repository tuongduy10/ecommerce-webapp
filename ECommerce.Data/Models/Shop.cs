using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Shop
    {
        public Shop()
        {
            Products = new HashSet<Product>();
            ShopBanks = new HashSet<ShopBank>();
            ShopBrands = new HashSet<ShopBrand>();
        }

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
        public int UserId { get; set; }
        public byte Status { get; set; }
        public int? DiscountId { get; set; }

        public virtual Discount Discount { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ShopBank> ShopBanks { get; set; }
        public virtual ICollection<ShopBrand> ShopBrands { get; set; }
    }
}
