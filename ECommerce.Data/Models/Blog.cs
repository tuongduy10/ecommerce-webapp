using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Blog
    {
        public int BlogId { get; set; }
        public byte? BlogPosition { get; set; }
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public byte? Status { get; set; }
    }
}
