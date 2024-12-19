﻿using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Services.Contacts.Econt.APIServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FarmFresh.Services.Econt.APIServices
{
    public class EcontShipmentService : IEcontShipmentService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        private readonly string credentials;

        private readonly string testApiUrl;
        private readonly string getShipmentStatusesEndpoint;

        private const string requestBodyFormat = "application/json";

        public EcontShipmentService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;

            if (_configuration["Econt:Username"].IsNullOrEmpty()
                || _configuration["Econt:Password"].IsNullOrEmpty()
                || _configuration["Econt:TestApiUrl"].IsNullOrEmpty()
                || _configuration["Econt:Endpoints:GetShipmentStatuses"].IsNullOrEmpty())
                throw new Exception("Econt test API authorization configuration is not properly set up.");

            string username = _configuration["Econt:Username"]!;
            string password = _configuration["Econt:Password"]!;

            credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            testApiUrl = _configuration["Econt:TestApiUrl"]!;
            getShipmentStatusesEndpoint = _configuration["Econt:Endpoints:GetShipmentStatuses"]!;
        }

        public async Task<GetShipmentStatusesResponse> GetShipmentStatusesAsync(GetShipmentStatusesRequest request)
        {
            var response = await GetResponseAsync(request, getShipmentStatusesEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var statusResponse = JsonConvert.DeserializeObject<GetShipmentStatusesResponse>(responseContent);
                
                if(statusResponse != null)
                    return statusResponse;
            }

            return null!;
        }

        private async Task<HttpResponseMessage> GetResponseAsync(RequestBase request, string endpoint)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, requestBodyFormat);

            var response = await _httpClient.PostAsync(testApiUrl + endpoint, content);
            return response;
        }
    }
}
