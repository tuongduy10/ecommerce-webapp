using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models
{
    public class Product
    {
        public Product() { }
        public Product(string name, decimal price) {
            this.name = name;
            this.price = price;
        }
        public string name { get; set; }
        public decimal price { get; set; }

    }
}
