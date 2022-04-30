using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public class ApiResponse
    {
        public bool isSucceed { get; set; }
        public string Message { get; set; }
        public Object ObjectData { get; set; }
    }
}
