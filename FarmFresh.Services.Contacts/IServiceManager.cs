namespace FarmFresh.Services.Contacts;

public interface IServiceManager
{
    IFarmerService FarmerService { get; }

    IAdminService AdminService { get; }

    ICategoryService CategoryService { get; }

    IProductService ProductService { get; }

    IProductPhotoService ProductPhotoService { get; }

    IReviewService ReviewService { get; }

    IOrderService OrderService { get; }

    ICartService CartService { get; }
}
