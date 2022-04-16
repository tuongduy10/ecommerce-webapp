using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.ProductUserImage.Dtos
{
    public class ProductUserImageModel
    {
        public int ProductUserImageId { get; set; }
        public string ProductUserImagePath { get; set; }
        public int? ProductId { get; set; }

    }
}
