using ECommerce.Application.Common;
using ECommerce.Application.Repositories;
using ECommerce.Application.Services.Common.DTOs;
using ECommerce.Application.Services.Common.DTOs.Requests;
using ECommerce.Application.Services.HttpClient;
using ECommerce.Data.Context;
using ECommerce.Data.Entities.Common;
using ECommerce.Dtos.Common;
using ECommerce.Utilities.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Common
{
    public class CommonService : ICommonService
    {
        private readonly ECommerceContext _DbContext;
        private readonly IRepositoryBase<Province> _provinceRepo;
        private readonly IRepositoryBase<District> _districtRepo;
        private readonly IRepositoryBase<Ward> _wardRepo;
        private readonly IHttpClientService _httpClientService;
        public CommonService(ECommerceContext DbContext,
            IHttpClientService httpClientService)
        {
            _DbContext = DbContext;
            _httpClientService = httpClientService;
            if (_provinceRepo == null)
                _provinceRepo = new RepositoryBase<Province>(_DbContext);
            if (_districtRepo == null)
                _districtRepo = new RepositoryBase<District>(_DbContext);
            if (_wardRepo == null)
                _wardRepo = new RepositoryBase<Ward>(_DbContext);
        }
        public async void AddFiles(IWebHostEnvironment webHostEnviroment, List<IFormFile> files, List<string> filesName, string path)
        {
            if (files != null && filesName != null && !String.IsNullOrEmpty(path))
            {
                string uploadsFolder = Path.Combine(webHostEnviroment.WebRootPath, path);
                for (int i = 0; i < files.Count; i++)
                {
                    string filePath = Path.Combine(uploadsFolder, filesName[i]);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        files[i].CopyTo(fileStream);
                    }
                }
            }
        }
        public async Task<Response<List<string>>> UploadAsync(UploadRequest request)
        {
            try
            {
                var formData = new MultipartFormDataContent();
                if (request.files != null && request.files.Count > 0)
                {
                    foreach (var file in request.files)
                    {
                        byte[] data;
                        using (var br = new BinaryReader(file.OpenReadStream()))
                        {
                            data = br.ReadBytes((int)file.OpenReadStream().Length);
                        }
                        ByteArrayContent bytes = new ByteArrayContent(data);
                        formData.Add(bytes, "files", file.FileName);
                    }
                }
                formData.Add(new StringContent(request.uploadType), "uploadType");

                return await _httpClientService.PostFormDataAsync<Response<List<string>>>("/common/upload", formData);
            }
            catch (Exception ex)
            {
                return new FailResponse<List<string>>(ex.Message);
            }
        }
        public void DeleteFiles(IWebHostEnvironment webHostEnviroment, List<string> fileNames, string path)
        {
            if (fileNames != null && fileNames.Count > 0 && !String.IsNullOrEmpty(path))
            {
                string uploadsFolder = Path.Combine(webHostEnviroment.WebRootPath, path);
                // remove previous image
                DirectoryInfo uploadDirectory = new DirectoryInfo(uploadsFolder);
                foreach (var fileName in fileNames)
                {
                    foreach (FileInfo file in uploadDirectory.GetFiles())
                    {
                        if (file.Name == fileName)
                        {
                            file.Delete();
                        }
                    }
                }
            }
        }
        public async Task<Response<string>> DeleteFilesAsync(RemoveFilesRequest request)
        {
            try
            {
                return await _httpClientService.PostAsync<RemoveFilesRequest, Response<string>>("/common/remove-files", request);
            }
            catch (Exception ex)
            {
                return new FailResponse<string>(ex.Message);
            }
        }
        public async Task<Response<List<ProvinceResponse>>> getProvinces()
        {
            try
            {
                var result = (await _provinceRepo.ToListAsync()).Select(_ => (ProvinceResponse)_).ToList();
                return new SuccessResponse<List<ProvinceResponse>>(result);
            }
            catch (Exception error)
            {
                return new FailResponse<List<ProvinceResponse>>(error.Message);
            }
        }
        public async Task<Response<List<DistrictResponse>>> getDistricts(DistrictGetRequest request)
        {
            try
            {
                var result = (await _districtRepo
                        .ToListAsyncWhere(_ => _.ProvinceCode == request.provinceCode))
                    .Select(_ => (DistrictResponse)_)
                    .ToList();
                return new SuccessResponse<List<DistrictResponse>>(result);
            }
            catch (Exception error)
            {
                return new FailResponse<List<DistrictResponse>>(error.Message);
            }
        }
        public async Task<Response<List<WardResponse>>> getWards(WardGetRequest request)
        {
            try
            {
                var result = (await _wardRepo
                        .ToListAsyncWhere(_ => _.DistrictCode == request.districtCode))
                    .Select(_ => (WardResponse)_)
                    .ToList();
                return new SuccessResponse<List<WardResponse>>(result);
            }
            catch (Exception error)
            {
                return new FailResponse<List<WardResponse>>(error.Message);
            }
        }
        public string EncryptString(HashRequest request)
        {
            return HashHelper.Encrypt(request.text, request.key);
        }
        public string DecryptString(HashRequest request)
        {
            return HashHelper.Decrypt(request.text, request.key);
        }
    }
}
