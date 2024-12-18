using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Services.Contacts.Econt.APIServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FarmFresh.Services.Econt.APIServices
{
    public class EcontLabelService : IEcontLabelService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        private readonly string credentials;

        private readonly string testApiUrl;
        private readonly string createLabelEndpoint;

        private const string requestBodyFormat = "application/json";

        public EcontLabelService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;

            if (_configuration["Econt:Username"].IsNullOrEmpty()
                || _configuration["Econt:Password"].IsNullOrEmpty()
                || _configuration["Econt:TestApiUrl"].IsNullOrEmpty()
                || _configuration["Econt:Endpoints:CreateLabel"].IsNullOrEmpty())
                throw new Exception("Econt test API authorization configuration is not properly set up.");

            string username = _configuration["Econt:Username"]!;
            string password = _configuration["Econt:Password"]!;

            credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            testApiUrl = _configuration["Econt:TestApiUrl"]!;
            createLabelEndpoint = _configuration["Econt:Endpoints:CreateLabel"]!;
        }

        public async Task<CreateLabelResponse> CreateLabel(CreateLabelRequest request)
        {
            var response = await GetResponseAsync(request, createLabelEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var labelResponse = JsonConvert.DeserializeObject<CreateLabelResponse>(responseContent);

                if(labelResponse != null)
                    return labelResponse;
            }

            return null!;
        }

        public async Task DeleteLabel()
        {
            //WIP
        }

        public async Task<double?> CalculateShipment(CalculateShipmentPriceRequest request)
        {
            var response = await GetResponseAsync(request, createLabelEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var labelResponse = JsonConvert.DeserializeObject<CalculateShipmentPriceResponse>(responseContent);

                if(labelResponse != null)
                    return labelResponse.Label!.TotalPrice;
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
