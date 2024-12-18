using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;

namespace FarmFresh.Services.Contacts.Econt
{
    public interface IStreetService
    {
        Task UpdateStreetsAsync();

        IQueryable<StreetDTO> FindStreetsByCityId(int cityId);
    }
}
