using System;
using System.Collections.Generic;

namespace ECommerce.Application.Services.User.Dtos
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
        public string UserPhone { get; set; }
        public List<string> UserRoles { get; set; }
        public bool Status { get; set; }
    }
}
