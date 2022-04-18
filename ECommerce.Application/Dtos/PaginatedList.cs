using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Dtos
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPage { get; set; }
        public PaginatedList(List<T> source, int count, int pageindex, int pagesize)
        {
            PageIndex = pageindex;
            TotalPage = (int)Math.Ceiling(count / (double)pagesize);
            AddRange(source);
        }
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageindex, int pagesize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
            return new PaginatedList<T>(items, count, pageindex, pagesize);
        }
    }
}
