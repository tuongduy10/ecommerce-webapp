using ECommerce.Application.Services.User.Enums;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.User
{
    public class UserRepository : RepositoryBase<Data.Models.User>,IUserRepository
    {
        public UserRepository(ECommerceContext DbContext) : base(DbContext)
        {

        }
        public bool IsAdmin(int userId)
        {
            var roles = Query(item => item.UserId == userId)
                .Select(item => item.UserRoles.Select(role => role.Role.RoleName))
                .FirstOrDefault();
            if (roles.Contains(RoleName.Admin))
                return true;
            return false;
        }
    }
}
