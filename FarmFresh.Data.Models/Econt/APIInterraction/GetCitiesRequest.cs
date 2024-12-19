using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class GetCitiesRequest : RequestBase
    {
        private string? _countryCode;

        [JsonProperty("countryCode")]
        public string CountryCode
        {
            get => _countryCode!;
            set
            {
                if (value.Length != 3)
                    throw new Exception("Country Code must be exactly 3 letters. (e.g. BGR, GRC, ROU..)");

                _countryCode = value;
            }
        }

        public GetCitiesRequest(string code3) => CountryCode = code3;
    }
}
