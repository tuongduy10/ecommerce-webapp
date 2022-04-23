using ECommerce.Application.Services.FilterProduct.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.FilterProduct.Dtos
{
    public class FilterModel
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public int CategoryId { get; set; }
        public List<Option> listOption { get; set; }
    }
}
