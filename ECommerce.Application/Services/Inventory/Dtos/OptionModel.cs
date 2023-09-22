using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Inventory.Dtos
{
    public class OptionModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<OptionValue> valueList { get; set; }
    }
    public class OptionValue
    {
        public int id { get; set; }
        public string name { get; set; }
        public int totalRecord { get; set; }
    }
}
