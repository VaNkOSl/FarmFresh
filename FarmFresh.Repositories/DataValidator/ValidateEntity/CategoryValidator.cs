using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using static FarmFresh.Commons.EntityValidationConstants.Categories;

namespace FarmFresh.Repositories.DataValidator.ValidateEntity;

internal sealed class CategoryValidator : IValidateEntity
{
    public Type GetValidatedType() => typeof(Category);

    public CRUDResult Validate(Entity entity) =>
        entity switch
        {
            Category category when ValidateProperties(category) => CRUDResult.Success,
            Category _ => CRUDResult.EntityValidationFailed,
            _ => CRUDResult.InvalidOperation
        };

    private bool ValidateProperties(Category category) =>
        ValidateName(category.Name);

    private bool ValidateName(string name) =>
        !string.IsNullOrWhiteSpace(name) &&
        name.Length > CategoryNameMinLength &&
        name.Length < CategoryNameMaxLength;
}
