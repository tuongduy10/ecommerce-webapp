using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class ReplyImage
    {
        public int RepyImageId { get; set; }
        public string Path { get; set; }
        public int? ReplyId { get; set; }

        public virtual Reply Reply { get; set; }
    }
}
