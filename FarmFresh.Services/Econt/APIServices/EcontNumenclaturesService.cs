using AutoMapper;
using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Econt.DTOs;
using FarmFresh.Services.Contacts.Econt.APIServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FarmFresh.Services.Econt.APIServices
{
    public class EcontNumenclaturesService : IEcontNumenclaturesService
    {
        private IConfiguration _configuration;
        private HttpClient _httpClient;
        private IMapper _mapper;

        private readonly string credentials;

        private readonly string testApiUrl;
        private readonly string getCountriesEndpoint;
        private readonly string getCitiesEndpoint;
        private readonly string getOfficesEndpoint;
        private readonly string getQuartersEndpoint;
        private readonly string getStreetsEndpoint;

        private const string requestBodyFormat = "application/json";

        public EcontNumenclaturesService(IConfiguration configuration, HttpClient httpClient, IMapper mapper)
        {
            _configuration = configuration;
            _httpClient = httpClient;

            string username = _configuration["Econt:Username"]!;
            string password = _configuration["Econt:Password"]!;

            //Econt API only authorizes requests with BASIC authentication
            credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            _mapper = mapper;

            testApiUrl = _configuration["Econt:TestApiUrl"]!;
            getCountriesEndpoint = _configuration["Econt:Endpoints:GetCountries"]!;
            getCitiesEndpoint = _configuration["Econt:Endpoints:GetCities"]!;
            getOfficesEndpoint = _configuration["Econt:Endpoints:GetOffices"]!;
            getStreetsEndpoint = _configuration["Econt:Endpoints:GetStreets"]!;
            getQuartersEndpoint = _configuration["Econt:Endpoints:GetQuarters"]!;
        }

        public async Task<List<CountryDTO>> GetCountriesAsync(GetCountriesRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, requestBodyFormat);

            var response = await _httpClient.PostAsync(testApiUrl + getCountriesEndpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var countriesResponse = JsonConvert.DeserializeObject<GetCountriesResponse>(responseContent);

                if (countriesResponse != null && countriesResponse.Countries != null)
                    return countriesResponse.Countries;
            }

            return null!;
        }

        public async Task<List<CityDTO>> GetCitiesAsync(GetCitiesRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, requestBodyFormat);
            
            var response  = await _httpClient.PostAsync(testApiUrl + getCitiesEndpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var citiesResponse = JsonConvert.DeserializeObject<GetCitiesResponse>(responseContent);

                if(citiesResponse != null && citiesResponse.Cities != null)
                    return citiesResponse.Cities;
            }

            return null!;
        }

        public async Task<List<OfficeDTO>> GetOfficesAsync(GetOfficesRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, requestBodyFormat);

            var response = await _httpClient.PostAsync(testApiUrl + getOfficesEndpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var officesResponse = JsonConvert.DeserializeObject<GetOfficesResponse>(responseContent);

                if (officesResponse != null && officesResponse.Offices != null)
                    return officesResponse.Offices;
            }

            return null!;
        }

        public Task<List<StreetDTO>> GetStreetsAsync()
        {
            //WIP
            return Task.FromResult(new List<StreetDTO>());
        }

        public Task<List<QuarterDTO>> GetQuartersAsync()
        {
            //WIP
            return Task.FromResult(new List<QuarterDTO>());
        }
    }
}
