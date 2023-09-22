using ECommerce.Application.BaseServices.PaymentMethod.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.BaseServices.PaymentMethod
{
    public class PaymentMethod : IPaymentMethod
    {
        public async Task<int> Create(PaymentMethodCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Delete(int PaymentMethodId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PaymentMethodModel>> getAll()
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(PaymentMethodModel request)
        {
            throw new NotImplementedException();
        }
    }
}
