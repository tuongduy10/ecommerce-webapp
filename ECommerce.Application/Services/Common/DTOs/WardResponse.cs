using ECommerce.Data.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Common.DTOs
{
    public class WardResponse
    {
        public string code { get; set; }
        public string name { get; set; }
        public string codeName { get; set; }
        public string districCode { get; set; }
        public static explicit operator WardResponse(Ward data)
        {
            return new WardResponse
            {
                code = data.Code,
                name = data.FullName,
                codeName = data.CodeName,
                districCode = data.DistrictCode
            };
        }
    }
}
