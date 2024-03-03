using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Inventory.Dtos
{
    public class CategoryModelRequest
    {
        public int id { get; set; }
        public string name { get; set; }
        public CategoryModelRequest()
        {
            id = -1;
            name = "";
        }
    }
}
