using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Product.Dtos
{
    public class DiscountModel
    {
        public int id { get; set; }
        public decimal? value { get; set; }
        public string code { get; set; }
        public DiscountTypeEnum type { get; set; } // global | shop | 
        public bool? isPercent { get; set; }
    }
    public enum DiscountTypeEnum
    {
        global,
        shop,
        brand,
    }
}
