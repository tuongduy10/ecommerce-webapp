using ECommerce.Application.Common;
using ECommerce.Application.Services.User.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.User
{
    public interface IUserService
    {
        Task<ApiResponse> SignIn(SignInRequest request);
    }
}
