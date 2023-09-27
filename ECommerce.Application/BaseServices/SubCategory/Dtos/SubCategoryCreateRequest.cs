using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.SubCategory.Dtos
{
    public class SubCategoryCreateRequest
    {
        public string SubCategoryName { get; set; }
        public int CategoryId { get; set; }
        public int SizeGuideId { get; set; }
    }
}
