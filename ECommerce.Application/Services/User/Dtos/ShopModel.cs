using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.User.Dtos
{
    public class ShopModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phoneNumber { get; set; }
        public string mail { get; set; }
        public string address { get; set; }
        public string wardCode { get; set; }
        public string districtCode { get; set; }
        public string cityCode { get; set; }
        public DateTime joinDate { get; set; }
        public byte tax { get; set; }
        public byte status { get; set; }
        public UserModel user { get; set; }
    }
}
