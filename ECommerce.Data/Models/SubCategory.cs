using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class SubCategory
    {
        public SubCategory()
        {
            Products = new HashSet<Product>();
            SubCategoryAttributes = new HashSet<SubCategoryAttribute>();
            SubCategoryOptions = new HashSet<SubCategoryOption>();
        }

        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<SubCategoryAttribute> SubCategoryAttributes { get; set; }
        public virtual ICollection<SubCategoryOption> SubCategoryOptions { get; set; }
    }
}
