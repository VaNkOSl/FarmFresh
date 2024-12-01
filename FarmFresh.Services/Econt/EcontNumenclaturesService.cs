using FarmFresh.Data.Models.Econt.APIInterractionClasses;
using FarmFresh.Data.Models.Econt.DTOs;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Services.Contacts.Econt;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FarmFresh.Services.Econt
{
    public class EcontNumenclaturesService : IEcontNumenclaturesService
    {
        private IConfiguration _configuration;
        private HttpClient _httpClient;

        private readonly string credentials;

        private readonly string testApiUrl;
        private readonly string getCountriesEndpoint;
        private readonly string getCititesEndpoint;
        private readonly string getOfficesEndpoint;
        private readonly string getQuartersEndpoint;
        private readonly string getStreetsEndpoint;

        public EcontNumenclaturesService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;

            string username = _configuration["Econt:Username"]!;
            string password = _configuration["Econt:Password"]!;

            //Econt API only authorizes requests with BASIC authentication
            credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            testApiUrl = _configuration["Econt:TestApiUrl"]!;
            getCountriesEndpoint = _configuration["Econt:Endpoints:GetCountries"]!;
            getCititesEndpoint = _configuration["Econt:Endpoints:GetCities"]!;
            getOfficesEndpoint = _configuration["Econt:Endpoints:GetOffices"]!;
            getStreetsEndpoint = _configuration["Econt:Endpoints:GetStreets"]!;
            getQuartersEndpoint = _configuration["Econt:Endpoints:GetQuarters"]!;
        }

        public async Task<List<CountryDTO>> GetCountries(GetCountriesRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(testApiUrl + getCountriesEndpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var countriesResponse = JsonConvert.DeserializeObject<GetCountriesResponse>(responseContent);

                if(countriesResponse != null && countriesResponse.Countries != null)
                    return countriesResponse.Countries;
            }

            return null!;
        }

        public Task<List<CityDTO>> GetCities()
        {
            throw new NotImplementedException();
        }

        public Task<List<OfficeDTO>> GetOffices()
        {
            throw new NotImplementedException();
        }

        public Task<List<StreetDTO>> GetStreets()
        {
            throw new NotImplementedException();
        }

        public Task<List<QuarterDTO>> GetQuarters()
        {
            throw new NotImplementedException();
        }
    }
}
