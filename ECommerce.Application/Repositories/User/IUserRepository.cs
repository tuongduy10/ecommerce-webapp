using ECommerce.Application.BaseServices.User.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.User
{
    public interface IUserRepository : IRepositoryBase<Data.Models.User>
    {
        Task<UserGetModel> GetUserInfo(int userId);
        bool IsAdmin(int userId);
    }
}
