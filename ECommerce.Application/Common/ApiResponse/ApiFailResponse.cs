using ECommerce.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public class ApiFailResponse : ApiResponse
    {
        public ApiFailResponse(string message)
        {
            isSucceed = false;
            Status = StatusConstant.FAIL;
            Message = message;
        }
    }
}
