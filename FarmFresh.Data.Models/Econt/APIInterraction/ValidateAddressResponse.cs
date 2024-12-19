using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class ValidateAddressResponse : ResponseBase
    {
        public AddressDTO? Address { get; set; }
        public string? ValidationStatus { get; set; }
    }
}
