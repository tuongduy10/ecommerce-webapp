using ECommerce.Application.Services.Configurations.Dtos;
using ECommerce.Application.Services.Configurations.Dtos.Header;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Configurations
{
    public class HeaderService : IHeaderService
    {
        private readonly ECommerceContext _DbContext;
        public HeaderService(ECommerceContext db)
        {
            _DbContext = db;
        }

        public async Task<List<HeaderModel>> getAll()
        {
            var query = from header in _DbContext.Headers 
                        orderby header.HeaderPosition
                        where header.Status == 1
                        select header;
            var list = await query.Select(i => new HeaderModel()
            {
                HeaderId = i.HeaderId,
                HeaderName = i.HeaderName,
                HeaderPosition = i.HeaderPosition,
                HeaderUrl = i.HeaderUrl,
                Status = i.Status
            }).ToListAsync();

            return list;
        }

        public async Task<int> Update(HeaderUpdateRequest request)
        {
            return 0;
        }
    }
}
