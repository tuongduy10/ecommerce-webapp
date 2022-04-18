using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Dtos
{
    public class PageResult<T>
    {
        public List<T> Items { get; set; }
        public int CurrentRecord { get; set; }
        public int TotalRecord { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}
