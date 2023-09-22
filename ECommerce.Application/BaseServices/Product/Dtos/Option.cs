using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Product.Dtos
{
    public class Option
    {
        public int id { get; set; } // optionId
        public string name { get; set; }
        public List<string> values { get; set; }
    }
}
