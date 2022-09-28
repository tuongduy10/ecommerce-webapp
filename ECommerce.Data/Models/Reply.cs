using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Reply
    {
        public Reply()
        {
            Interests = new HashSet<Interest>();
            ReplyImages = new HashSet<ReplyImage>();
        }

        public int ReplyId { get; set; }
        public int? ReplyIndex { get; set; }
        public string Comment { get; set; }
        public int? RateId { get; set; }
        public int? UserId { get; set; }
        public int? TagedUserId { get; set; }

        public virtual Rate Rate { get; set; }
        public virtual User TagedUser { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Interest> Interests { get; set; }
        public virtual ICollection<ReplyImage> ReplyImages { get; set; }
    }
}
