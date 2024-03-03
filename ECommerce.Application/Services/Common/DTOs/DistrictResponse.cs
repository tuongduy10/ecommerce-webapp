using ECommerce.Data.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services.Common.DTOs
{
    public class DistrictResponse
    {
        public string code { get; set; }
        public string name { get; set; }
        public string codeName { get; set; }
        public string provinceCode { get; set; }
        public static explicit operator DistrictResponse(District data)
        {
            return new DistrictResponse
            {
                code = data.Code,
                name = data.FullName,
                codeName = data.CodeName,
                provinceCode = data.ProvinceCode
            };
        }
    }
}
