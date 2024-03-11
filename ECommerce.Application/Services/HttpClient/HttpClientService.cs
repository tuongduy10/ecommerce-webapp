using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ECommerce.Utilities.Helpers;
using ECommerce.Utilities.Constants;

namespace ECommerce.Application.Services.HttpClient
{
    public class HttpClientService : IHttpClientService
    {
        private string _url;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpClientService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<TResponse> GetAsync<TResponse>(string url)
        {
            string address = _configuration.GetValue<string>(SystemConstant.AppSettings.AssetsServerAddress);
            string uri = HashHelper.Decrypt(address);
            //string token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            string token = "";

            // Create HttpClient with base address and authorization header
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Send GET request
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Read response content
            string responseBody = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response
            TResponse responseObject = JsonConvert.DeserializeObject<TResponse>(responseBody);
            return responseObject;
        }
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest payload)
        {
            string address = _configuration.GetValue<string>(SystemConstant.AppSettings.AssetsServerAddress);
            string uri = HashHelper.Decrypt(address);
            //string token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            string token = "";

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"/api{url}", content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseBody);
        }
        public async Task<TResponse> PostFormDataAsync<TResponse>(string url, MultipartFormDataContent formData)
        {
            string address = _configuration.GetValue<string>(SystemConstant.AppSettings.AssetsServerAddress);
            string uri = HashHelper.Decrypt(address);
            //string token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            string token = "";

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.PostAsync($"/api{url}", formData);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseBody);
        }
    }
}
