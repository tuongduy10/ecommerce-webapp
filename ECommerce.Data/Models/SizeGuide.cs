using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class SizeGuide
    {
        public SizeGuide()
        {
            SubCategories = new HashSet<SubCategory>();
        }

        public int SizeGuideId { get; set; }
        public string SizeGuide1 { get; set; }
        public bool? IsBaseSizeGuide { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
