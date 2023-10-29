using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.User.Enums
{
    public enum enumRole
    {
        Admin = 1,
        Seller = 2,
        Buyer = 3,
    }
    public class RoleName
    {
        public const string Admin = "Admin";
        public const string Seller = "Seller";
        public const string Buyer = "Buyer";
    }
}
