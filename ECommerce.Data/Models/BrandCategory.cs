using System;
using System.Collections.Generic;

namespace ECommerce.Data.Models
{
    public partial class BrandCategory
    {
        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
    }
}
