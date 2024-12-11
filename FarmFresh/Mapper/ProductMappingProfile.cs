using AutoMapper;
using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Admin;
using FarmFresh.ViewModels.Product;

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
           .ForMember(dest => dest.Photos, opt =>
               opt.MapFrom(src => src.ProductPhotos != null && src.ProductPhotos.Any()
                   ? src.ProductPhotos.Select(photo => new ProductPhotosDto
                   {
                       Id = photo.Id,
                       FilePath = "/uploads/" + Path.GetFileName(photo.FilePath),
                       ProductId = photo.ProductId
                   })
                   : new List<ProductPhotosDto>()));

        CreateMap<(IEnumerable<AllProductsDto> products, MetaData metaData, string searchTerm), ProductsListViewModel>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.products))
            .ForMember(dest => dest.MetaData, opt => opt.MapFrom(src => src.metaData))
            .ForMember(dest => dest.SearchTerm, opt => opt.MapFrom(src => src.searchTerm));

        CreateMap<Product, ProductPreDeleteDto>()
            .ForMember(dest => dest.PhotoString, opt =>
             opt.MapFrom(src => src.ProductPhotos.FirstOrDefault() != null ?
             src.ProductPhotos.FirstOrDefault().FilePath : string.Empty))
            .ForMember(dest => dest.Photos, opt =>
             opt.MapFrom(src => src.ProductPhotos.Select(photo => new ProductPhotosDto
             {
                 FilePath = photo.FilePath,
                 ProductId = photo.ProductId,
             })));

        CreateMap<Product, AdminAllProductDto>()
            .ForMember(dest => dest.FarmerPhoneNumber, opt => opt.MapFrom(src => src.Farmer.PhoneNumber))
            .ForMember(dest => dest.FarmerPhoto, opt => opt.MapFrom(src => src.Farmer.Photo))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.PhotoString, opt => opt.MapFrom(src =>
                src.Farmer.Photo != null ? Convert.ToBase64String(src.Farmer.Photo) : string.Empty))
            .ForMember(dest => dest.Photos, opt =>
            opt.MapFrom(src => src.ProductPhotos != null && src.ProductPhotos.Any()
            ? src.ProductPhotos.Select(photo => new ProductPhotosDto
            {
                Id = photo.ProductId,
                FilePath = "/uploads/" + Path.GetFileName(photo.FilePath),
                ProductId = photo.ProductId
            })
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
            .ForMember(dest => dest.Photos, opt =>
            opt.MapFrom(src => src.ProductPhotos != null && src.ProductPhotos.Any()
            ? src.ProductPhotos.Select(photo => new ProductPhotosDto
            {
                Id = photo.Id,
                FilePath = "/uploads/" + Path.GetFileName(photo.FilePath),
                ProductId = photo.ProductId
            })
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
