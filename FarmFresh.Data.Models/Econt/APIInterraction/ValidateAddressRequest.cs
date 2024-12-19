using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;
using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class ValidateAddressRequest(AddressDTO address) : RequestBase
    {
        [JsonProperty("address")]
        public AddressDTO Address { get; set; } = address;
    }
}
