using ECommerce.Application.Common;
using ECommerce.Application.Services.Common.DTOs;
using ECommerce.Application.Services.Common.DTOs.Requests;
using ECommerce.Dtos.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Common
{
    public interface ICommonService
    {
        void AddFiles(IWebHostEnvironment webHostEnviroment, List<IFormFile> files, List<string> filesName, string path);
        void DeleteFiles(IWebHostEnvironment webHostEnviroment, List<string> fileNames, string path);
        Task<Response<List<string>>> UploadAsync(UploadRequest request);
        Task<Response<string>> DeleteFilesAsync(RemoveFilesRequest request);
        Task<Response<List<ProvinceResponse>>> getProvinces();
        Task<Response<List<DistrictResponse>>> getDistricts(DistrictGetRequest request);
        Task<Response<List<WardResponse>>> getWards(WardGetRequest request);
        string EncryptString(HashRequest request);
        string DecryptString(HashRequest request);
    }
}
