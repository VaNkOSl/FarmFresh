using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;

namespace FarmFresh.Repositories.DataValidator;

public interface IValidateEntity
{
    Type GetValidatedType();

    CRUDResult Validate(Entity entity);
}
