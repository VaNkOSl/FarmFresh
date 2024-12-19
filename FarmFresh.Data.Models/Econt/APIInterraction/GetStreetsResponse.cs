using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class GetStreetsResponse : ResponseBase
    {
        public List<StreetDTO>? Streets { get; set; }
    }
}
