using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class GetQuartersResponse : ResponseBase
    {
        public List<QuarterDTO>? Quarters { get; set; }
    }
}
