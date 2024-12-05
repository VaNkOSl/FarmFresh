using AutoMapper;
using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Product;

namespace FarmFresh.Mapper;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        //CreateMap<CreateProductDto, Product>()
        //    .ForMember(dest => dest.FarmerId, opt => opt.Ignore())
        //    .ForMember(dest => dest.Photo, opt => 
        //               opt.MapFrom(src => src.PhotoFile != null 
        //               ? ConvertToByteArray(src.PhotoFile) : new byte[0]));

        CreateMap<CreateProductDto, Product>()
          .ForMember(dest => dest.FarmerId, opt => opt.Ignore());

        CreateMap<Product, AllProductsDto>()
           .ForMember(dest => dest.Photos, opt =>
               opt.MapFrom(src => src.ProductPhotos != null && src.ProductPhotos.Any()
                   ? src.ProductPhotos.Select(photo => new ProductPhotosDto
                   {
                       Id = photo.Id,
                       FilePath = "/uploads/" + Path.GetFileName(photo.FilePath), // Пътя до снимката
                       ProductId = photo.ProductId
                   })
                   : new List<ProductPhotosDto>()));

        CreateMap<ProductPhoto, ProductPhotosDto>()
            .ForMember(dest => dest.FilePath, opt =>
                opt.MapFrom(src => "/uploads/" + Path.GetFileName(src.FilePath)));

        //CreateMap<Product, AllProductsDto>()
        //    .ForMember(dest => dest.PhotoString, opt => 
        //    opt.MapFrom(src => src.Photo != null ? 
        //    Convert.ToBase64String(src.Photo) : null));

        CreateMap<(IEnumerable<AllProductsDto> products, MetaData metaData, string searchTerm), ProductsListViewModel>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.products))
            .ForMember(dest => dest.MetaData, opt => opt.MapFrom(src => src.metaData))
            .ForMember(dest => dest.SearchTerm, opt => opt.MapFrom(src => src.searchTerm));

    }

    private byte[] ConvertToByteArray(IFormFile file)
    {
        using(var memoryStream = new  MemoryStream())
        {
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
