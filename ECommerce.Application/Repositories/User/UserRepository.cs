using ECommerce.Application.BaseServices.User.Dtos;
using ECommerce.Application.BaseServices.User.Enums;
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
        public async Task<UserGetModel> GetUserInfo(int userId)
        {
            var result = await Query(i => i.UserId == userId)
                .Select(i => new UserGetModel()
                {
                    UserId = i.UserId,
                    UserAddress = i.UserAddress,
                    UserCityCode = i.UserCityCode,
                    UserDistrictCode = i.UserDistrictCode,
                    UserFullName = i.UserFullName,
                    UserPhone = i.UserPhone,
                })
                .FirstOrDefaultAsync();
            if (result != null) return result;
            return null;
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
