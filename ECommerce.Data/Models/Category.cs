using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Category
    {
        public Category()
        {
            Brands = new HashSet<Brand>();
            SubCategories = new HashSet<SubCategory>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Brand> Brands { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
