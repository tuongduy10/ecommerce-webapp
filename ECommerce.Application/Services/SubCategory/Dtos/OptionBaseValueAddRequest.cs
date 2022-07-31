using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.SubCategory.Dtos
{
    public class OptionBaseValueAddRequest
    {
        public List<int> ids { get; set; } // option ids
        public string value { get; set; }
    }
}
