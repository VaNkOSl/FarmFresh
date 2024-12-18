using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using static FarmFresh.Commons.EntityValidationConstants.Orders;
using static FarmFresh.Commons.EntityValidationConstants.User;
using static FarmFresh.Commons.EntityValidationConstants.Farmers;

namespace FarmFresh.Repositories.DataValidator.ValidateEntity;

public class OrderValidator : IValidateEntity
{
    public Type GetValidatedType() => typeof(Order);

    public CRUDResult Validate(Entity entity) =>
        entity switch
        {
            Order order when ValidateProperties(order) => CRUDResult.Success,
            Order _ => CRUDResult.EntityValidationFailed,
            _ => CRUDResult.InvalidOperation
        };

    private bool ValidateProperties(Order order) =>
        ValidateName(order.FirstName, order.LastName) &&
        ValidateAdress(order.Adress) &&
        ValidatePhoneNumber(order.PhoneNumber);

    private bool ValidateName(string firstName, string lastName) =>
        !string.IsNullOrWhiteSpace(firstName) &&
        !string.IsNullOrWhiteSpace(lastName) &&
        firstName.Length >= UserFirstNameMinLength &&
        firstName.Length <= UserFirstNameMaxLength &&
        lastName.Length >= UserLastNameMinLength &&
        lastName.Length <= UserLastNameMaxLength;

    private bool ValidateAdress(string adress) =>
        !string.IsNullOrWhiteSpace(adress) &&
        adress.Length >= OrderAdressMinLength &&
        adress.Length <= OrderAdressMaxLength;

    private bool ValidatePhoneNumber(string phoneNumber) =>
        !string.IsNullOrWhiteSpace(phoneNumber)
        && phoneNumber.Length < FarmerPhoneNumberMaxLength
        && phoneNumber.Length >= FarmerPhoneNumberMinLength;
}
