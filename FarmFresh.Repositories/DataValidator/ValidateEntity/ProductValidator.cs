using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using static FarmFresh.Commons.EntityValidationConstants.Products;

namespace FarmFresh.Repositories.DataValidator.ValidateEntity;

internal sealed class ProductValidator : IValidateEntity
{
    public Type GetValidatedType() => typeof(Product);

    public CRUDResult Validate(Entity entity) =>
        entity switch
        {
            Product product when ValidateProperties(product) => CRUDResult.Success,
            Product _ => CRUDResult.EntityValidationFailed,
            _ => CRUDResult.InvalidOperation,
        };

    private bool ValidateProperties(Product product) =>
        ValidateName(product.Name) &&
        ValidateOrigin(product.Origin) &&
        ValidatePrice(product.Price) &&
        ValidateStockQuantity(product.StockQuantity) &&
        ValidateHarvestDate(product.HarvestDate) &&
        ValidateExpirationDate(product.HarvestDate, product.ExpirationDate);

    private bool ValidateName(string name) =>
        !string.IsNullOrWhiteSpace(name) &&
        name.Length >= ProductNameMinLength &&
        name.Length <= ProductNameMaxLength;

    private bool ValidateOrigin(string origin) =>
        !string.IsNullOrWhiteSpace(origin) &&
        origin.Length >= ProductOriginMinLength &&
        origin.Length <= ProductOriginMaxLength;

    private bool ValidatePrice(decimal price) =>
        price > 0 && price <= decimal.MaxValue;

    private bool ValidateStockQuantity(int quantity) =>
        quantity > 0;

    private bool ValidateHarvestDate(DateTime harvestDate) =>
      harvestDate <= DateTime.UtcNow;

    private bool ValidateExpirationDate(DateTime harvestDate, DateTime expirationDate) =>
        expirationDate > harvestDate && expirationDate > DateTime.UtcNow;
}
