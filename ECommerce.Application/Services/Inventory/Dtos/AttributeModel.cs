using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Inventory.Dtos
{
    public class AttributeModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public static explicit operator AttributeModel(Data.Entities.Attribute data)
        {
            return new AttributeModel
            {
                id = data.AttributeId,
                name = data.AttributeName
            };
        }
    }
}
