using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Inventory.Dtos
{
    public class ShopModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public ShopModel() { }
        public ShopModel(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
