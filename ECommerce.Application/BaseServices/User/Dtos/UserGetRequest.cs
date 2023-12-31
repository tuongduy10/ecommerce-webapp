using ECommerce.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.User.Dtos
{
    public class UserGetRequest : PageRequest
    {
        public int userId { get; set; }
        public bool all { get; set; }
        public bool isSystemAccount { get; set; }
        public bool isSeller { get; set; }
        public bool isAdmin { get; set; }
        public bool isOnline { get; set; }
        public bool isOffline { get; set; }
        public UserGetRequest()
        {
            userId = -1;
        }
    }
}
