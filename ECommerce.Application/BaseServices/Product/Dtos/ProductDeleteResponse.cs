using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Product.Dtos
{
    public class ProductDeleteResponse
    {
        public List<string> systemImages { get; set; }
        public List<string> userImages { get; set; }
        public List<string> ratingImages { get; set; }
    }
}
