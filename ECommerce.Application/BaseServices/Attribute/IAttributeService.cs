using ECommerce.Application.BaseServices.Attribute.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.Attribute
{
    public interface IAttributeService
    {
        Task<int> Create(AttributeCreateRequest request);
        Task<int> Update(AttributeModel request);
        Task<int> Delete(int AttributeId);
        Task<List<AttributeModel>> getAll();
    }
}
