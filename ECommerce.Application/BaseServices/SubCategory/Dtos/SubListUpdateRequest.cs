using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.SubCategory.Dtos
{
    public class SubListUpdateRequest
    {
        public List<int> ids { get; set;} // subcategory ids
        public string name { get; set; } // option name/attribute
    }
}
