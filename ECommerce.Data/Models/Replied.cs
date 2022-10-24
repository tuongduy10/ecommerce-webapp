using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Replied
    {
        public int? RepliedId { get; set; }
        public string Comment { get; set; }
        public string CreateDate { get; set; }
    }
}
