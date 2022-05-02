using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Account.Dtos
{
    public class UserUpdateRequest
    {
        public int UserId { get; set; }
        public string UserMail { get; set; }
        public string UserFullName { get; set; }
        public string UserAddress { get; set; }
        public string UserWardCode { get; set; }
        public string UserDistrictCode { get; set; }
        public string UserCityCode { get; set; }
    }
}
