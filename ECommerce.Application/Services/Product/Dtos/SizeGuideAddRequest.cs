using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class SizeGuideAddRequest
    {
        public List<int> ids { get; set; }
        public string content { get; set; }
        public int id { get; set; } // for update
    }
}
