using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Product.Dtos
{
    public class ProductRecordModel
    {
        public int CurrentPage { get; set; }
        public int CurrentRecord { get; set; }
        public int TotalPage { get; set; }
        public int TotalRecord { get; set; }
        public List<ProductModel> Items { get; set; }
    }
}
