using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Econt.APIInterraction;
using Newtonsoft.Json.Linq;

namespace FarmFresh.Services.Contacts.Econt;

public interface IEcontManagmentService
{
	Task<CreateLabelResponse> CreateLabel(Order order, bool trackChanges);

	Task<JObject> GetAddressByLatAndLongAsync(double latitude, double longitude);

	Task<IEnumerable<string>> GetCitiesAsync(string searchTerm);

	Task<IEnumerable<string>> GetEcontOfficesAsync(string cityName);

	Task<decimal> CalculatePrice(Order order, bool trackChanges);
}
