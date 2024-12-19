using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class GetCountriesResponse : ResponseBase
    {
        public List<CountryDTO>? Countries { get; set; }
    }
}
