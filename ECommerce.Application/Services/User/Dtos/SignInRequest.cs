using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.User.Dtos
{
    public class SignInRequest
    {
        public SignInRequest() { }
        public SignInRequest(string userphone, string password)
        {
            UserPhone = userphone;
            Password = password;
        }
        public string UserPhone { get; set; }
        public string Password { get; set; }
    }
}
