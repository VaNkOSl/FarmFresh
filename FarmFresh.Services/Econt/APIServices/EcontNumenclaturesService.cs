using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;
using FarmFresh.Services.Contacts.Econt.APIServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FarmFresh.Services.Econt.APIServices
{
    public class EcontNumenclaturesService : IEcontNumenclaturesService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        private readonly string credentials;

        private readonly string testApiUrl;
        private readonly string getCountriesEndpoint;
        private readonly string getCitiesEndpoint;
        private readonly string getOfficesEndpoint;
        private readonly string getQuartersEndpoint;
        private readonly string getStreetsEndpoint;

        private const string requestBodyFormat = "application/json";

        public EcontNumenclaturesService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;

            if (_configuration["Econt:Username"].IsNullOrEmpty()
                || _configuration["Econt:Password"].IsNullOrEmpty()
                || _configuration["Econt:TestApiUrl"].IsNullOrEmpty()
                || _configuration["Econt:Endpoints:GetCountries"].IsNullOrEmpty()
                || _configuration["Econt:Endpoints:GetCities"].IsNullOrEmpty()
                || _configuration["Econt:Endpoints:GetOffices"].IsNullOrEmpty()
                || _configuration["Econt:Endpoints:GetStreets"].IsNullOrEmpty()
                || _configuration["Econt:Endpoints:GetQuarters"].IsNullOrEmpty())
                throw new Exception("Econt test API authorization configuration is not properly set up.");

            string username = _configuration["Econt:Username"]!;
            string password = _configuration["Econt:Password"]!;

            credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            testApiUrl = _configuration["Econt:TestApiUrl"]!;
            getCountriesEndpoint = _configuration["Econt:Endpoints:GetCountries"]!;
            getCitiesEndpoint = _configuration["Econt:Endpoints:GetCities"]!;
            getOfficesEndpoint = _configuration["Econt:Endpoints:GetOffices"]!;
            getStreetsEndpoint = _configuration["Econt:Endpoints:GetStreets"]!;
            getQuartersEndpoint = _configuration["Econt:Endpoints:GetQuarters"]!;
        }

        public async Task<List<CountryDTO>> GetCountriesAsync(GetCountriesRequest request)
        {
            var response = await GetResponseAsync(request, getCountriesEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var countriesResponse = JsonConvert.DeserializeObject<GetCountriesResponse>(responseContent);

                if (countriesResponse != null && !countriesResponse.Countries.IsNullOrEmpty())
                    return countriesResponse.Countries!;
            }

            return null!;
        }

        public async Task<List<CityDTO>> GetCitiesAsync(GetCitiesRequest request)
        {
            var response = await GetResponseAsync(request, getCitiesEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var citiesResponse = JsonConvert.DeserializeObject<GetCitiesResponse>(responseContent);

                if(citiesResponse != null && !citiesResponse.Cities.IsNullOrEmpty())
                    return citiesResponse.Cities!;
            }

            return null!;
        }

        public async Task<List<OfficeDTO>> GetOfficesAsync(GetOfficesRequest request)
        {
            var response = await GetResponseAsync(request, getOfficesEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var officesResponse = JsonConvert.DeserializeObject<GetOfficesResponse>(responseContent);

                if (officesResponse != null && !officesResponse.Offices.IsNullOrEmpty())
                    return officesResponse.Offices!;
            }

            return null!;
        }

        public async Task<List<StreetDTO>> GetStreetsAsync(GetStreetsRequest request)
        {
            var response = await GetResponseAsync(request, getStreetsEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var streetsResponse = JsonConvert.DeserializeObject<GetStreetsResponse>(responseContent);

                if(streetsResponse != null && !streetsResponse.Streets.IsNullOrEmpty())
                    return streetsResponse.Streets!;
            }

            return null!;
        }

        public async Task<List<QuarterDTO>> GetQuartersAsync(GetQuartersRequest request)
        {
            var response = await GetResponseAsync(request, getQuartersEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var quartersResponse = JsonConvert.DeserializeObject<GetQuartersResponse>(responseContent);

                if(quartersResponse != null && !quartersResponse.Quarters.IsNullOrEmpty())
                    return quartersResponse.Quarters!;
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
