using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class NewOption
    {
        public int id { get; set; } // optionId
        public List<string> values { get; set; }
    }
}
