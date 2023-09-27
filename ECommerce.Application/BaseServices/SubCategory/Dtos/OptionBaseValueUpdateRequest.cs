using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.SubCategory.Dtos
{
    public class OptionBaseValueUpdateRequest
    {
        public int id { get; set; } //option id
        public string name { get; set; } // option name
        public List<int> ids { get; set; } // value id
    }
}
