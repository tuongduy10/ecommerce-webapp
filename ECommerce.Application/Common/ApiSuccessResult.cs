using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult(string message, T resultObj)
        {
            IsSuccessed = true;
            Message = message;
            ResultObj = resultObj;
        }
    }
}
