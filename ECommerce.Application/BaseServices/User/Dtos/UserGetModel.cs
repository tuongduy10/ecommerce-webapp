using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerce.Application.BaseServices.User.Dtos
{
    public class UserGetModel
    {
        public int UserId { get; set; }
        public string UserMail { get; set; }
        public string UserFullName { get; set; }
        public DateTime UserJoinDate { get; set; }
        public string UserAddress { get; set; }
        public string UserWardCode { get; set; }
        public string UserDistrictCode { get; set; }
        public string UserCityCode { get; set; }
        public string UserWardName { get; set; }
        public string UserDistrictName { get; set; }
        public string UserCityName { get; set; }
        public string UserPhone { get; set; }
        public List<string> UserRoles { get; set; }
        public bool Status { get; set; }
        public bool isSystemAccount { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastOnline { get; set; }
        public string LastOnlineLabel { get; set; }
        public static explicit operator UserGetModel(Data.Models.User data)
        {
            return new UserGetModel()
            {
                UserId = data.UserId,
                UserMail = data.UserMail,
                UserFullName = data.UserFullName,
                UserPhone = data.UserPhone,
                UserJoinDate = (DateTime)data.UserJoinDate,
                UserAddress = data.UserAddress,
                UserDistrictCode = data.UserDistrictCode,
                UserWardCode = data.UserWardCode,
                UserCityCode = data.UserCityCode,
                UserCityName = data.UserCityName,
                UserDistrictName = data.UserDistrictName,
                UserWardName = data.UserWardName,
                IsOnline = (bool)data.IsOnline,
                isSystemAccount = (bool)data.IsSystemAccount,
                Status = (bool)data.Status,
            };
        }
    }
}
