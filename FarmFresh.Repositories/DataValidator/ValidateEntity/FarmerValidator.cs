using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using static FarmFresh.Commons.EntityValidationConstants.Farmers;

namespace FarmFresh.Repositories.DataValidator.ValidateEntity;

internal sealed class FarmerValidator : IValidateEntity
{
    public Type GetValidatedType() => typeof(Farmer);

    public CRUDResult Validate(Entity entity) =>
        entity switch
        {
            Farmer farmer when ValidateProperties(farmer) => CRUDResult.Success,
            Farmer _ => CRUDResult.EntityValidationFailed,
            _ => CRUDResult.InvalidOperation
        };

    private bool ValidateProperties(Farmer farmer) =>
        ValidateDescriptionLength(farmer.FarmDescription) &&
        ValidateLocationLength(farmer.Location) &&
        ValidatePhoneNumber(farmer.PhoneNumber) &&
        ValidateEgn(farmer.Egn);


    private bool ValidateDescriptionLength(string description) =>
        !string.IsNullOrWhiteSpace(description) 
        && description.Length < FarmerDescriptionMaxLength 
        && description.Length >= FarmerDescriptionMinLength;

    private bool ValidateLocationLength(string location) =>
        !string.IsNullOrWhiteSpace(location)
        && location.Length < FarmerLocationMaxLegth
        && location.Length >= FarmerLocationMinLegth;

    private bool ValidatePhoneNumber(string phoneNumber) =>
        !string.IsNullOrWhiteSpace(phoneNumber)
        && phoneNumber.Length < FarmerPhoneNumberMaxLength
        && phoneNumber.Length >= FarmerPhoneNumberMinLength;

    private bool ValidateEgn(string egn) =>
        !string.IsNullOrEmpty(egn)
        && egn.Length <= FarmerEgnMaxLength
        && egn.Length >= FarmerEgnMinLength;
}
