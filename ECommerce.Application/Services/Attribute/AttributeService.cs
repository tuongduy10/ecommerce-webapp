using ECommerce.Application.Services.Attribute.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Attribute
{
    public class AttributeService : IAttributeService
    {
        public async Task<int> Create(AttributeCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Delete(int AttributeId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AttributeModel>> getAll()
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(AttributeModel request)
        {
            throw new NotImplementedException();
        }
    }
}
