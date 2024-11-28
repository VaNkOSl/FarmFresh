namespace FarmFresh.Services.Contacts;

public interface IServiceManager
{
    IFarmerService FarmerService { get; }

    IAdminService AdminService { get; }

    ICategoryService CategoryService { get; }
}
