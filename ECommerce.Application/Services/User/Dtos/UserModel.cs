using ECommerce.Application.BaseServices.User.Dtos;
using ECommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.User.Dtos
{
    public class UserModel
    {
        public int id { get; set; }
        public string mail { get; set; }
        public string fullName { get; set; }
        public DateTime joinDate { get; set; }
        public string address { get; set; }
        public string wardCode { get; set; }
        public string districtCode { get; set; }
        public string cityCode { get; set; }
        public string phone { get; set; }
        public List<string> roles { get; set; }
        public bool status { get; set; }
        public bool isSystemAccount { get; set; }
        public bool isOnline { get; set; }
        public DateTime lastOnline { get; set; }
        public List<Shop> shops { get; set; }
        public static explicit operator UserModel(UserGetModel data)
        {
            return new UserModel
            {
                id = data.UserId,
                fullName = data.UserFullName,
                cityCode = data.UserCityCode,
                districtCode = data.UserDistrictCode,
                wardCode = data.UserWardCode,
                phone = data.UserPhone,
                address = data.UserAddress,
                mail = data.UserMail,
                shops = data.Shops
            };
        }
    }
}
