using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Inventory.Dtos
{
    public class OptionModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<OptionValueModel> values { get; set; }
    }
    public class OptionValueModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int totalRecord { get; set; } // total products with this value
        public bool isBase { get; set; }
    }
}
