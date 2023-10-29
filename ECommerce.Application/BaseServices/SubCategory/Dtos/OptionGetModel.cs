using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.SubCategory.Dtos
{
    public class OptionGetModel
    {
        public int id { get; set; } // main and default
        public string name { get; set; } // main and default
        public List<OptionValueGetModel> values { get; set; }
    }
}
