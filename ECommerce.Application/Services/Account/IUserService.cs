using ECommerce.Application.Common;
using ECommerce.Application.Services.Account.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Account
{
    public interface IUserService
    {
        Task<ApiResponse> SignIn(SignInRequest request);
        Task<ApiResponse> SignUp(SignUpRequest request);
        Task<bool> checkUserPhoneNumber(string PhoneNumber);
    }
}
