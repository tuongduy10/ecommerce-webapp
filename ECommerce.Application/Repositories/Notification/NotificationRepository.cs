using ECommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories.Notification
{
    public class NotificationRepository : RepositoryBase<string>, INotificationRepository
    {
        public NotificationRepository(ECommerceContext DbContext):base(DbContext)
        {

        }
    }
}
