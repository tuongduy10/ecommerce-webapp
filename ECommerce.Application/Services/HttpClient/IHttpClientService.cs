using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.HttpClient
{
    public interface IHttpClientService
    {
        Task<TResponse> GetAsync<TResponse>(string url);
        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data);
        Task<TResponse> PostFormDataAsync<TResponse>(string url, MultipartFormDataContent formData);
    }
}
