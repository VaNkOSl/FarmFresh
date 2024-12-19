using FarmFresh.Data.Models.Econt.APIInterraction;

namespace FarmFresh.Services.Contacts.Econt.APIServices
{
    public interface IEcontShipmentService
    {
        Task<GetShipmentStatusesResponse> GetShipmentStatusesAsync(GetShipmentStatusesRequest request);
    }
}
