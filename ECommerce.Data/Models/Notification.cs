using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public int? SeenderId { get; set; }
        public int? ReceiverId { get; set; }
        public string Title { get; set; }
        public string TextContent { get; set; }
        public string Link { get; set; }
        public DateTime? Time { get; set; }
        public bool? Seen { get; set; }
        public bool? Deleted { get; set; }
        public int? TypeId { get; set; }

        public virtual User Receiver { get; set; }
        public virtual User Seender { get; set; }
        public virtual NotificationType Type { get; set; }
    }
}
