using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.FilterProduct.Dtos
{
    public class Option
    {
        public int OptionId { get; set; }
        public string OptionName { get; set; }
        public List<OptionValue> listOptionValue { get; set; }
    }
}
