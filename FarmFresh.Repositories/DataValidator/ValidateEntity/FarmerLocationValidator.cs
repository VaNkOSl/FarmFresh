using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using static FarmFresh.Commons.EntityValidationConstants.FarmerLocations;

namespace FarmFresh.Repositories.DataValidator.ValidateEntity;

internal sealed class FarmerLocationValidator : IValidateEntity
{
    public Type GetValidatedType() => typeof(FarmerLocation);

    public CRUDResult Validate(Entity entity) =>
        entity switch
        {
            FarmerLocation farmerLocation when ValidateProperties(farmerLocation) => CRUDResult.Success,
            FarmerLocation _ => CRUDResult.EntityValidationFailed,
            _ => CRUDResult.InvalidOperation
        };
    private bool ValidateProperties(FarmerLocation farmerLocation) =>
         ValidateLatitude(farmerLocation.Latitude) &&
         ValidateLongitude(farmerLocation.Longitude) &&
         ValidateTitle(farmerLocation.Title);

    private bool ValidateLatitude(double latitude) =>
        latitude > LatitudeMinValue && latitude < LatitudeMaxValue;

    private bool ValidateLongitude(double longitude) =>
        longitude > LongitudeMinValue && longitude < LongitudeMaxValue;

    private bool ValidateTitle(string? title) =>
        !string.IsNullOrWhiteSpace(title) &&
        title.Length > TitleMinLength &&
        title.Length < TitleMaxLength;

}
