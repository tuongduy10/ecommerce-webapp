using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.User.Dtos
{
    public class UserUpdateRequest
    {
        public int id { get; set; }
        public bool status { get; set; } // true: enable, false: disable
    }
}
