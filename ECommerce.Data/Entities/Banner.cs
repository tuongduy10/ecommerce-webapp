using System;
using System.Collections.Generic;

namespace ECommerce.Data.Entities
{
    public partial class Banner
    {
        public int BannerId { get; set; }
        public string BannerPath { get; set; }
        public byte? Status { get; set; }
    }
}
