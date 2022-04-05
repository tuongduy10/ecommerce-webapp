using System;
using System.Collections.Generic;

#nullable disable

namespace ECommerce.Data.Models
{
    public partial class Option
    {
        public Option()
        {
            ProductOptions = new HashSet<ProductOption>();
            SubCategoryOptions = new HashSet<SubCategoryOption>();
        }

        public int OptionId { get; set; }
        public string OptionName { get; set; }

        public virtual ICollection<ProductOption> ProductOptions { get; set; }
        public virtual ICollection<SubCategoryOption> SubCategoryOptions { get; set; }
    }
}
