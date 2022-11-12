using ECommerce.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public class ApiSuccessResponse : ApiResponse
    {
        public ApiSuccessResponse()
        {
            isSucceed = true;
            Status = SystemConstant.SUCCESS;
        }
        public ApiSuccessResponse(string message)
        {
            isSucceed = true;
            Status = SystemConstant.SUCCESS;
            Message = message;
        }
        public ApiSuccessResponse(string message, Object objData)
        {
            isSucceed = true;
            Message = message;
            Status = SystemConstant.SUCCESS;
            ObjectData = objData;
        }
    }
}
