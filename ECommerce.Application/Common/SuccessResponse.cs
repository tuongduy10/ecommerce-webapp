using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public class SuccessResponse<T> : Response<T>
    {
        public SuccessResponse(string message)
        {
            isSucceed = true;
            Message = message;
        }
        public SuccessResponse(string message, T data)
        {
            isSucceed = true;
            Message = message;
            Data = data;
        }
    }
}
