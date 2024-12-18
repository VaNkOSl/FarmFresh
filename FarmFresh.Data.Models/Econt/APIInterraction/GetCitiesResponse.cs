using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class GetCitiesResponse : ResponseBase
    {
        public List<CityDTO>? Cities { get; set; }
    }
}
