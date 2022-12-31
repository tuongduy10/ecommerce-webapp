using System;
using System.Collections.Generic;

namespace ECommerce.Data.Models
{
    public partial class SubCategoryOption
    {
        public int SubCategoryId { get; set; }
        public int OptionId { get; set; }

        public virtual Option Option { get; set; }
        public virtual SubCategory SubCategory { get; set; }
    }
}
