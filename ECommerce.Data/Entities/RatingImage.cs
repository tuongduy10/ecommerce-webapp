using System;
using System.Collections.Generic;

namespace ECommerce.Data.Entities
{
    public partial class RatingImage
    {
        public int RatingImageId { get; set; }
        public string RatingImagePath { get; set; }
        public int? RateId { get; set; }

        public virtual Rate Rate { get; set; }
    }
}
