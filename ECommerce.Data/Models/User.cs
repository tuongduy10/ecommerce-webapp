using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
            Rates = new HashSet<Rate>();
            Shops = new HashSet<Shop>();
            UserRoles = new HashSet<UserRole>();
        }

        public int UserId { get; set; }
        public string UserMail { get; set; }
        public string UserFullName { get; set; }
        public DateTime? UserJoinDate { get; set; }
        public string UserAddress { get; set; }
        public string UserWardCode { get; set; }
        public string UserDistrictCode { get; set; }
        public string UserCityCode { get; set; }
        public string UserPhone { get; set; }
        public string Password { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
