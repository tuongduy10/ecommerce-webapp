using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Product.Enum
{
    public enum enumProductType
    {
        Available = 1,
        PreOrder = 2,
    }
    public class ProductTypeConst
    {
        public const string AvailableName = "Hàng có sẵn";
        public const string PreOrderName = "Hàng đặt trước";
    }
}
