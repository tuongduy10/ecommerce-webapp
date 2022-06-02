using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Rate
    {
        public Rate()
        {
            RatingImages = new HashSet<RatingImage>();
        }

        public int RateId { get; set; }
        public int? RateValue { get; set; }
        public string Comment { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<RatingImage> RatingImages { get; set; }
    }
}
