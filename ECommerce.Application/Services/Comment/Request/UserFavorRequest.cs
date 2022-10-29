using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Comment.Request
{
    public class UserFavorRequest
    {
        public int id { get; set; }
        public bool liked { get; set; }
    }
}
