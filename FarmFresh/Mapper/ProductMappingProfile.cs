using AutoMapper;
using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Admin;
using FarmFresh.ViewModels.Farmer;
using FarmFresh.ViewModels.Product;
using FarmFresh.ViewModels.Review;

namespace FarmFresh.Mapper;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.FarmerId, opt => opt.Ignore())
            .ForMember(dest => dest.ProductStatus, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => true));

        CreateMap<ProductPhoto, ProductPhotosDto>()
            .ForMember(dest => dest.FilePath, opt =>
                opt.MapFrom(src => "/uploads/" + Path.GetFileName(src.FilePath)));

        CreateMap<Product, UpdateProductDto>()
          .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
          .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.CurrentPhotos, opt => opt.MapFrom(src => src.ProductPhotos))
          .ReverseMap();

        CreateMap<Product, AllProductsDto>()
        .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
        .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name))
        .ForCtorParam("Price", opt => opt.MapFrom(src => src.Price))
        .ForCtorParam("StockQuantity", opt => opt.MapFrom(src => src.StockQuantity))
        .ForCtorParam("Photos", opt => opt.MapFrom(src => src.ProductPhotos));

        CreateMap<(IEnumerable<AllProductsDto> products, MetaData metaData, string searchTerm), ProductsListViewModel>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.products))
                .ForMember(dest => dest.MetaData, opt => opt.MapFrom(src => src.metaData))
                .ForMember(dest => dest.SearchTerm, opt => opt.MapFrom(src => src.searchTerm));

        CreateMap<Product, ProductPreDeleteDto>()
            .ForMember(dest => dest.PhotoString, opt =>
             opt.MapFrom(src => src.ProductPhotos.FirstOrDefault() != null ?
             src.ProductPhotos.FirstOrDefault().FilePath : string.Empty))
            .ForMember(dest => dest.Photos, opt => opt.MapFrom(src =>
              src.ProductPhotos != null && src.ProductPhotos.Any()
                ? src.ProductPhotos.Select(photo => new ProductPhotosDto(
                    photo.Id,
                    "/uploads/" + Path.GetFileName(photo.FilePath),
                    photo.Photo,
                    photo.ProductId
                )).ToList()
                : new List<ProductPhotosDto>()));

        CreateMap<Product, AdminAllProductDto>()
            .ForMember(dest => dest.FarmerPhoneNumber, opt => opt.MapFrom(src => src.Farmer.PhoneNumber))
            .ForMember(dest => dest.FarmerPhoto, opt => opt.MapFrom(src => src.Farmer.Photo))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.PhotoString, opt => opt.MapFrom(src =>
                src.Farmer.Photo != null ? Convert.ToBase64String(src.Farmer.Photo) : string.Empty))
          .ForMember(dest => dest.Photos, opt => opt.MapFrom(src =>
              src.ProductPhotos != null && src.ProductPhotos.Any()
                ? src.ProductPhotos.Select(photo => new ProductPhotosDto(
                    photo.Id,
                    "/uploads/" + Path.GetFileName(photo.FilePath),
                    photo.Photo,
                    photo.ProductId
                )).ToList()
                : new List<ProductPhotosDto>()));

        CreateMap<Product, AdminRejectProductViewModel>()
            .ForMember(dest => dest.FarmerName, opt =>
            opt.MapFrom(src => src.Farmer.User.FirstName + " " + src.Farmer.User.LastName))
            .ForMember(dest => dest.FarmerEmail, opt =>
            opt.MapFrom(src => src.Farmer.User.Email))
            .ForMember(dest => dest.FarmerPhoto, opt =>
            opt.MapFrom(src => src.Farmer.Photo))
            .ForMember(dest => dest.FarmerPhoneNumber, opt =>
            opt.MapFrom(src => src.Farmer.PhoneNumber))
            .ForMember(dest => dest.EmailFrom, opt => opt.MapFrom(src => "contactfarmfresh2024@abv.bg"))
            .ForMember(dest => dest.EmailTo, opt => opt.MapFrom(src => src.Farmer.User.Email))
            .ForMember(dest => dest.Photos, opt => opt.MapFrom(src =>
              src.ProductPhotos != null && src.ProductPhotos.Any()
                ? src.ProductPhotos.Select(photo => new ProductPhotosDto(
                    photo.Id,
                    "/uploads/" + Path.GetFileName(photo.FilePath),
                    photo.Photo,
                    photo.ProductId
                )).ToList()
                : new List<ProductPhotosDto>()));


        CreateMap<Product, ProductDetailsDto>()
        .ForMember(dest => dest.CategoryName, opt =>
            opt.MapFrom(src => src.Category != null ? src.Category.Name : "Unknown"))
        .ForMember(dest => dest.Farmer, opt => opt.MapFrom(src =>
            new FarmerProfileViewModel(
                $"{src.Farmer.User.FirstName} {src.Farmer.User.LastName}",
                src.Farmer.PhoneNumber,
                src.Farmer.Location,
                src.Farmer.FarmDescription, 
                src.Farmer.Photo != null ? Convert.ToBase64String(src.Farmer.Photo) : string.Empty,
                src.Farmer.Id)))
        .ForMember(dest => dest.Photos, opt => opt.MapFrom(src =>
                src.ProductPhotos != null && src.ProductPhotos.Any()
                    ? src.ProductPhotos.Select(photo => new ProductPhotosDto(
                        photo.Id,
                        "/uploads/" + Path.GetFileName(photo.FilePath),
                        photo.Photo,
                        photo.ProductId
                    )).ToList()
                    : new List<ProductPhotosDto>()))
        .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src =>
            src.Reviews != null && src.Reviews.Any()
                ? src.Reviews.Select(review => new ProductReviewDto
                {
                    Id = review.Id,
                    Content = review.Content,
                    ProductId = review.ProductId,
                    Rating = review.Rating,
                    ReviewDate = review.ReviewDate,
                    UserId = review.UserId,
                    UserFullName = review.User != null
                        ? $"{review.User.FirstName} {review.User.LastName}"
                        : "Anonymous"
                }).ToList()
                : new List<ViewModels.Review.ProductReviewDto>()));

        CreateMap<Product, MineProductsDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name))
            .ForCtorParam("Price", opt => opt.MapFrom(src => src.Price))
            .ForCtorParam("StockQuantity", opt => opt.MapFrom(src => src.StockQuantity))
            .ForCtorParam("ProductStatus", opt => opt.MapFrom(src => src.ProductStatus))
            .ForCtorParam("CategoryName", opt => opt.MapFrom(src => src.Category.Name))
            .ForCtorParam("FarmerId", opt => opt.MapFrom(src => src.FarmerId))
            .ForCtorParam("Photos", opt => opt.MapFrom(src =>
             src.ProductPhotos != null && src.ProductPhotos.Any()
                    ? src.ProductPhotos.Select(photo => new ProductPhotosDto(
                        photo.Id,
                        "/uploads/" + Path.GetFileName(photo.FilePath),
                        photo.Photo,
                        photo.ProductId
                    )).ToList()
                    : new List<ProductPhotosDto>()));

        CreateMap<Product, AllReviewDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Reviews.FirstOrDefault().Id))
            .ForCtorParam("ProductName", opt => opt.MapFrom(src => src.Name))
            .ForCtorParam("ProductId", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Photos", opt => opt.MapFrom(src =>
             src.ProductPhotos != null && src.ProductPhotos.Any()
                    ? src.ProductPhotos.Select(photo => new ProductPhotosDto(
                        photo.Id,
                        "/uploads/" + Path.GetFileName(photo.FilePath),
                        photo.Photo,
                        photo.ProductId
                    )).ToList()
                    : new List<ProductPhotosDto>()));
    }

    private byte[] ConvertToByteArray(IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}