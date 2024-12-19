using FarmFresh.Data.Models.Econt.APIInterraction;

namespace FarmFresh.Services.Contacts.Econt.APIServices
{
    public interface IEcontLabelService
    {
        Task<CreateLabelResponse> CreateLabel(CreateLabelRequest request);
        
        Task<double?> CalculateShipment(CalculateShipmentPriceRequest request);
    }
}
