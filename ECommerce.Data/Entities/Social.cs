using System;
using System.Collections.Generic;

namespace ECommerce.Data.Entities
{
    public partial class Social
    {
        public int SocialId { get; set; }
        public byte? Position { get; set; }
        public string Icon { get; set; }
        public string SocialName { get; set; }
        public string SocialUrl { get; set; }
        public byte? Status { get; set; }
    }
}
