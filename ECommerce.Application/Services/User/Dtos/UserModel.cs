using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.User.Dtos
{
    public class UserModel
    {
        public int id { get; set; }
        public string mail  { get; set; }
        public string fullName { get; set; }
        public DateTime joinDate { get; set; }
        public AddressModel addressInfo { get; set; }
        public string phone { get; set; }
        public List<string> roles { get; set; }
        public bool status { get; set; }
        public bool isSystemAccount { get; set; }
        public bool isOnline { get; set; }
        public DateTime lastOnline { get; set; }
    }
    public class AddressModel
    {
        public string address { get; set; }
        public string wardCode { get; set; }
        public string districtCode { get; set; }
        public string cityCode { get; set; }
    }
}
