using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public class ApiSucceedResponse : ApiResponse
    {
        public ApiSucceedResponse(string message, Object objData)
        {
            isSucceed = true;
            Message = message;
            ObjectData = objData;
        }
    }
}
