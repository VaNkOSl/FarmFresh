using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Services.Contacts.Econt.APIServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FarmFresh.Services.Econt.APIServices;

public class EcontLabelService : IEcontLabelService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    private readonly string credentials;

    private readonly string testApiUrl;
    private readonly string createLabelEndpoint;
    private readonly string deleteLabelEndpoint;

    private const string requestBodyFormat = "application/json";

    public EcontLabelService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;

        if (_configuration["Econt:Username"].IsNullOrEmpty()
            || _configuration["Econt:Password"].IsNullOrEmpty()
            || _configuration["Econt:TestApiUrl"].IsNullOrEmpty()
            || _configuration["Econt:Endpoints:CreateLabel"].IsNullOrEmpty()
            || _configuration["Econt:Endpoints:DeleteLabels"].IsNullOrEmpty())
            throw new Exception("Econt test API authorization configuration is not properly set up.");

        string username = _configuration["Econt:Username"]!;
        string password = _configuration["Econt:Password"]!;

        credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        testApiUrl = _configuration["Econt:TestApiUrl"]!;
        createLabelEndpoint = _configuration["Econt:Endpoints:CreateLabel"]!;
        deleteLabelEndpoint = _configuration["Econt:Endpoints:DeleteLabels"]!;
    }

    public async Task<CreateLabelResponse> CreateLabelAsync(CreateLabelRequest request)
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

    public async Task<DeleteLabelsResponse> DeleteLabelAsync(DeleteLabelsRequest request)
    {
        var response = await GetResponseAsync(request, deleteLabelEndpoint);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var deletionResponse = JsonConvert.DeserializeObject<DeleteLabelsResponse>(responseContent);

            if(deletionResponse != null)
                return deletionResponse;
        }

        return null!;
    }

    public async Task<double?> CalculateShipmentAsync(CalculateShipmentPriceRequest request)
    {
        var response = await GetResponseAsync(request, createLabelEndpoint);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var labelResponse = JsonConvert.DeserializeObject<CalculateShipmentPriceResponse>(responseContent);

            if(labelResponse != null && labelResponse.Label!.TotalPrice != null)
                return labelResponse.Label!.TotalPrice;
        }

        return null!;
    }

    private async Task<HttpResponseMessage> GetResponseAsync(RequestBase request, string endpoint)
    {
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, requestBodyFormat);

        var response = await _httpClient.PostAsync(testApiUrl + endpoint, content);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
        }
        return response;
    }
}
