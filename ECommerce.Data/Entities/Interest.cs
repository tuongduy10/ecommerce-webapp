using System;
using System.Collections.Generic;

namespace ECommerce.Data.Entities
{
    public partial class Interest
    {
        public int RateId { get; set; }
        public int UserId { get; set; }
        public bool? Liked { get; set; }

        public virtual Rate Rate { get; set; }
        public virtual User User { get; set; }
    }
}
