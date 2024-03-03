using System;
using System.Collections.Generic;

namespace ECommerce.Data.Entities
{
    public partial class Header
    {
        public int HeaderId { get; set; }
        public byte? HeaderPosition { get; set; }
        public string HeaderName { get; set; }
        public string HeaderUrl { get; set; }
        public byte? Status { get; set; }
    }
}
