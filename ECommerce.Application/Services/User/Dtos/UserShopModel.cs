using ECommerce.Data.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.User.Dtos
{
    public class UserShopModel
    {
        public int id { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string cityCode { get; set; }
        public string districtCode { get; set; }
        public string wardCode { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string rePassword { get; set; }
        public List<int> shopIds { get; set; }
        public UserShopModel()
        {
            id = -1;
            fullName = "";
            email = "";
            cityCode = "";
            districtCode = "";
            wardCode = "";
            address = "";
            phone = "";
            password = "";
            rePassword = "";
            shopIds = new List<int>();
        }
    }
}
