using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Inventory.Dtos
{
    public class InventoryRequest
    {
        public int productId { get; set; }
        public int subCategoryId { get; set; }
        public int brandId { get; set; }
        public bool isBase { get; set; }
        public InventoryRequest()
        {
            productId = -1;
            subCategoryId = -1;
            brandId = -1;
            isBase = false;
        }
    }
}
