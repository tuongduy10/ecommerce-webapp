using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class RatingImage
    {
        public int RatingImageId { get; set; }
        public string RatingImagePath { get; set; }
        public int? RateId { get; set; }

        public virtual Rate Rate { get; set; }
    }
}
