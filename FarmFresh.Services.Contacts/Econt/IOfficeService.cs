using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;

namespace FarmFresh.Services.Contacts.Econt
{
    public interface IOfficeService
    {
        Task UpdateOfficesAsync();

        IQueryable<OfficeDTO> FindOfficesByCityId(int cityId);
    }
}
