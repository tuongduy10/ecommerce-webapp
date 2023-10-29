using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Inventory.Dtos
{
    public class AttributeRequest
    {
        public int productId { get; set; }
        public int subCategoryId { get; set; }
        public AttributeRequest()
        {
            productId = -1;
            subCategoryId = -1;
        }
    }
}
