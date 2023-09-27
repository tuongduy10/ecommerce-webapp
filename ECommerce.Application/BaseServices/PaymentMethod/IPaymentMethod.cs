using ECommerce.Application.BaseServices.PaymentMethod.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.PaymentMethod
{
    public interface IPaymentMethod
    {
        Task<int> Create(PaymentMethodCreateRequest request);
        Task<int> Update(PaymentMethodModel request);
        Task<int> Delete(int PaymentMethodId);
        Task<List<PaymentMethodModel>> getAll();
    }
}
