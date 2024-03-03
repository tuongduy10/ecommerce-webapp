using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Entities
{
    public partial class Category
    {
        public Category()
        {
            BrandCategories = new HashSet<BrandCategory>();
            SubCategories = new HashSet<SubCategory>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<BrandCategory> BrandCategories { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
