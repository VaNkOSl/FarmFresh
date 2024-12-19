using FarmFresh.Data.Models.Econt.APIInterraction;

namespace FarmFresh.Services.Contacts.Econt.APIServices
{
    public interface IEcontLabelService
    {
        Task<CreateLabelResponse> CreateLabelAsync(CreateLabelRequest request);

        Task<DeleteLabelsResponse> DeleteLabelAsync(DeleteLabelsRequest request);
        
        Task<double?> CalculateShipmentAsync(CalculateShipmentPriceRequest request);
    }
}
