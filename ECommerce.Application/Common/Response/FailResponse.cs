using ECommerce.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public class FailResponse<T> : Response<T>
    {
        public FailResponse(string message)
        {
            isSucceed = false;
            Status = SystemConstant.FAIL;
            Message = message;
        }
    }
}
