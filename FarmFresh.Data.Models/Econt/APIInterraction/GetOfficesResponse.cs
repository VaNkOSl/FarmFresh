using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class GetOfficesResponse : ResponseBase
    {
        public List<OfficeDTO>? Offices { get; set; }
    }
}
