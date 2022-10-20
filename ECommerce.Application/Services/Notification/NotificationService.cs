using ECommerce.Application.Common;
using ECommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private ECommerceContext _DbContext;
        public NotificationService(ECommerceContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<ApiResponse> Add()
        {
            try
            {
                return new ApiSuccessResponse();
            }
            catch (Exception error)
            {
                return new ApiFailResponse(error.ToString());
            }
        }
    }
}
