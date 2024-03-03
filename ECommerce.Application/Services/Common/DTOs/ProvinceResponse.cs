using ECommerce.Data.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Common.DTOs
{
    public class ProvinceResponse
    {
        public string code { get; set; }
        public string name { get; set; }
        public string codeName { get; set; }
        public static explicit operator ProvinceResponse(Province data)
        {
            return new ProvinceResponse
            {
                code = data.Code,
                name = data.Name,
                codeName = data.CodeName
            };
        }
    }
}
