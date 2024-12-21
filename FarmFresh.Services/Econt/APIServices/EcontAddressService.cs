using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Services.Contacts.Econt.APIServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FarmFresh.Services.Econt.APIServices
{
    public class EcontAddressService : IEcontAddressService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        private readonly string credentials;

        private readonly string testApiUrl;
        private readonly string validateAddressEndpoint;

        private const string requestBodyFormat = "application/json";

        public EcontAddressService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;

            if (_configuration["Econt:Username"].IsNullOrEmpty()
                || _configuration["Econt:Password"].IsNullOrEmpty()
                || _configuration["Econt:TestApiUrl"].IsNullOrEmpty()
                || _configuration["Econt:Endpoints:ValidateAddress"].IsNullOrEmpty())
                throw new Exception("Econt test API authorization configuration is not properly set up.");

            string username = _configuration["Econt:Username"]!;
            string password = _configuration["Econt:Password"]!;

            credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            testApiUrl = _configuration["Econt:TestApiUrl"]!;
            validateAddressEndpoint = _configuration["Econt:Endpoints:ValidateAddress"]!;
        }

        public async Task<bool> ValidateAddressAsync(ValidateAddressRequest request)
        {
            //For some reason Econt's ValidateAddress method on their end
            //requires Country entities to not have assigned IDs
            if (request.Address.City!.Country!.Id != null)
                request.Address.City!.Country!.Id = null;

            var response = await GetResponseAsync(request, validateAddressEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var validation = JsonConvert.DeserializeObject<ValidateAddressResponse>(responseContent);

                return validation!.ValidationStatus == "normal" || validation.ValidationStatus == "processed";
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Status Code: {response.StatusCode}");
            Console.WriteLine($"Reason Phrase: {response.ReasonPhrase}");
            Console.WriteLine($"Error Content: {errorContent}");

            return false;
        }

        private async Task<HttpResponseMessage> GetResponseAsync(RequestBase request, string endpoint)
        {
            var json = JsonConvert.SerializeObject(request);
            Console.WriteLine(json);
            var content = new StringContent(json, Encoding.UTF8, requestBodyFormat);

            var response = await _httpClient.PostAsync(testApiUrl + endpoint, content);
            return response;
        }
    }
}
