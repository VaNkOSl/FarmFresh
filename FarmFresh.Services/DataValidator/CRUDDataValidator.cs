using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Services.Contacts;

namespace FarmFresh.Services.DataValidator
{
    public class CRUDDataValidator : IValidateEntity
    {
        private readonly IServiceManager _serviceProvider;
        private readonly List<IValidateEntity> _validators = new();

        public CRUDDataValidator(IServiceManager serviceProvider)
        {
            _serviceProvider = serviceProvider;
           // _validators.Add(new UserValidator(_serviceProvider.AccountService));
        }

        public Type GetValidatedType() => typeof(CRUDDataValidator);

        public CRUDResult Validate(Entity entity) =>
            _validators.Where(x => x.GetValidatedType() == entity.GetType()).FirstOrDefault()?.Validate(entity) ??
            CRUDResult.Success;
    }
}
