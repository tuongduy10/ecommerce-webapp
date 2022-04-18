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
        public PaginatedList(List<T> source, int count, int index, int size)
        {
            PageIndex = index;
            TotalPage = (int)Math.Ceiling(count / (double)size);
            AddRange(source);
        }
        public static PaginatedList<T> Create(IQueryable<T> source, int index, int size)
        {
            var count = source.Count();
            var items = source.Skip((index - 1) * size).Take(size).ToList();

            return new PaginatedList<T>(items, count, index, size);
        }
    }
}
