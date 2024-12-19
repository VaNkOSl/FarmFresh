using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;

namespace FarmFresh.Services.Contacts.Econt
{
    public interface IQuarterService
    {
        Task UpdateQuartersAsync();

        IQueryable<QuarterDTO> FindQuartersByCityId(int cityId);
    }
}
