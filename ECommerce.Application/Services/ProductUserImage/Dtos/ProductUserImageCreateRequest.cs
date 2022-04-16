using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.ProductUserImage.Dtos
{
    public class ProductUserImageCreateRequest
    {
        public string ProductUserImagePath { get; set; }
        public int? ProductId { get; set; }

    }
}
