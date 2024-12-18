using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Repositories.DataValidator.ValidateEntity;

namespace FarmFresh.Repositories.DataValidator;

public class CRUDDataValidator : IValidateEntity
{
    private readonly List<IValidateEntity> _validators = new();

    public CRUDDataValidator()
    {
        _validators.Add(new FarmerValidator());
        _validators.Add(new FarmerLocationValidator());
        _validators.Add(new CategoryValidator());
        _validators.Add(new ProductValidator());
    }

    public Type GetValidatedType() => typeof(CRUDDataValidator);

    public CRUDResult Validate(Entity entity) =>
        _validators.Where(x => x.GetValidatedType() == entity.GetType()).FirstOrDefault()?.Validate(entity) ??
        CRUDResult.Success;
}
