using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Enums
{
    public enum RoleEnum
    {
        Admin = 1,
        Seller = 2,
        Buyer = 3,
    }
    public class RoleName
    {
        public const string ADMIN = "Admin";
        public const string SELLER = "Seller";
        public const string BUYER = "Buyer";
    }
}
