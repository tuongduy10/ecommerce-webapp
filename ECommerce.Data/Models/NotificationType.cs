using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class NotificationType
    {
        public NotificationType()
        {
            Notifications = new HashSet<Notification>();
        }

        public int TypeId { get; set; }
        public string TypeText { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
