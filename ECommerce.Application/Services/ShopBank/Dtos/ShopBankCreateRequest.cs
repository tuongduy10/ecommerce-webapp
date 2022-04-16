using ECommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.ShopBank.Dtos
{
    public class ShopBankCreateRequest
    {
        public string ShopBankName { get; set; }
        public decimal ShopAccountNumber { get; set; }
        public int ShopId { get; set; }
    }
}
