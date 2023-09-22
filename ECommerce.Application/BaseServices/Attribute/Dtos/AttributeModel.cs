using ECommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Attribute.Dtos
{
    public class AttributeModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }
}