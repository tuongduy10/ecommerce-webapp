
using System.Collections.Generic;

namespace ECommerce.Application.BaseServices.User.Dtos
{
    public class UserUpdateRequest
    {
        public int UserId { get; set; }
        public string UserMail { get; set; }
        public string UserFullName { get; set; }
        public string UserAddress { get; set; }
        public string UserPhone { get; set; }
        public string UserWardCode { get; set; }
        public string UserDistrictCode { get; set; }
        public string UserCityCode { get; set; }
        public bool Status { get; set; }
        public List<int> ShopIds { get; set; }
    }
}
