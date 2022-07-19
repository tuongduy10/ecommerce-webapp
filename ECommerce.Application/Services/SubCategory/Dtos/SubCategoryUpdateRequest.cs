using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.SubCategory.Dtos
{
    public class SubCategoryUpdateRequest
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<int> attrIds { get; set; }
        public List<int> optIds { get; set; }
    }
}
