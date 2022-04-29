using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public class ApiFailResult<T> : ApiResult<T>
    {
        public ApiFailResult(string message)
        {
            IsSuccessed = false;
            Message = message;
        }
    }
}
