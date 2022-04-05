using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Social
    {
        public int SocialId { get; set; }
        public string SocialName { get; set; }
        public string SocialUrl { get; set; }
        public byte? Status { get; set; }
    }
}
