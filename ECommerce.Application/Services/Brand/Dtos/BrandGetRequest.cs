using System.Collections.Generic;

namespace ECommerce.Application.Services.Brand.Dtos
{
    public class BrandGetRequest
    {
        public List<string> alphabet { get; set; }
        public int CategoryId { get; set; }
    }
}
