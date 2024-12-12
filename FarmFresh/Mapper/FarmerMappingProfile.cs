using AutoMapper;
using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Admin;
using FarmFresh.ViewModels.Farmer;
using FarmFresh.ViewModels.Product;

namespace FarmFresh.Mapper;

public class FarmerMappingProfile : Profile
{
    public FarmerMappingProfile()
    {
        CreateMap<FarmerForCreationDto, Farmer>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.FarmerStatus, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.PhotoFile != null ? ConvertToByteArray(src.PhotoFile) : new byte[0]))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.BirthDate));

        CreateMap<FarmerForUpdatingDto, Farmer>()
            .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.PhotoFile != null ? ConvertToByteArray(src.PhotoFile) : new byte[0]))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.BirthDate))
            .ReverseMap();

        CreateMap<FarmerForUpdatingDto, Farmer>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.PhotoFile != null ? ConvertToByteArray(src.PhotoFile) : new byte[0]))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.BirthDate));

        CreateMap<Farmer, FarmersViewModel>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("FullName", opt => opt.MapFrom(src =>
                src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : "No Name"))
            .ForCtorParam("PhoneNumber", opt => opt.MapFrom(src =>
                src.PhoneNumber ?? "No Phone Number"))
            .ForCtorParam("PhotoString", opt => opt.MapFrom(src =>
                src.Photo != null ? Convert.ToBase64String(src.Photo) : string.Empty))
            .ForCtorParam("Photo", opt => opt.MapFrom(src => src.Photo))
            .ForCtorParam("ProductCount", opt => opt.MapFrom(src => src.OwnedProducts.Count));

        CreateMap<Farmer, FarmerProfileViewModel>()
            .ForCtorParam("FullName", opt => opt.MapFrom(src =>
                src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : "No Name"))
            .ForCtorParam("PhoneNumber", opt => opt.MapFrom(src => src.PhoneNumber))
            .ForCtorParam("Location", opt => opt.MapFrom(src => src.Location))
            .ForCtorParam("FarmDescription", opt => opt.MapFrom(src => src.FarmDescription))
            .ForCtorParam("PhotoString", opt => opt.MapFrom(src =>
                src.Photo != null && src.Photo.Length > 0
                    ? Convert.ToBase64String(src.Photo)
                    : null));

        CreateMap<(IEnumerable<FarmersViewModel> farmers, MetaData metaData, string searchTerm), FarmersListViewModel>()
            .ForMember(dest => dest.Farmers, opt => opt.MapFrom(src => src.farmers))
            .ForMember(dest => dest.MetaData, opt => opt.MapFrom(src => src.metaData))
            .ForMember(dest => dest.SearchTerm, opt => opt.MapFrom(src => src.searchTerm));

        CreateMap<Farmer, AdminAllFarmersDto>()
            .ForMember(dest => dest.FarmerFullName, opt => opt.MapFrom(src =>
                src.User != null ? src.User.FirstName + " " + src.User.LastName : "No Name"))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src =>
                src.PhoneNumber != null ? src.PhoneNumber : "No Phone Number"))
            .ForMember(dest => dest.PhotoString, opt => opt.MapFrom(src =>
                src.Photo != null ? Convert.ToBase64String(src.Photo) : string.Empty));

        CreateMap<Farmer, AdminRejectFarmerDto>()
            .ForMember(dest => dest.FarmerFullName, opt => opt.MapFrom(src =>
                src.User != null ? src.User.FirstName + " " + src.User.LastName : "No Name"))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src =>
                src.PhoneNumber != null ? src.PhoneNumber : "No Phone Number"))
            .ForMember(dest => dest.PhotoString, opt => opt.MapFrom(src =>
                src.Photo != null ? Convert.ToBase64String(src.Photo) : string.Empty))
            .ForMember(dest => dest.EmailFrom, opt => opt.MapFrom(src => "contactfarmfresh2024@abv.bg"))
            .ForMember(dest => dest.EmailTo, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.FarmerEmail, opt => opt.MapFrom(src => src.User.Email));

        CreateMap<(IEnumerable<AdminAllFarmersDto> farmers, MetaData metaData, string searchTerm), AdminFarmerListViewModel>()
            .ForMember(dest => dest.Farmers, opt => opt.MapFrom(src => src.farmers))
            .ForMember(dest => dest.MetaData, opt => opt.MapFrom(src => src.metaData))
            .ForMember(dest => dest.SearchTerm, opt => opt.MapFrom(src => src.searchTerm));

        CreateMap<Farmer, FarmerDetailsDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id != Guid.Empty ? src.Id : Guid.NewGuid()))
            .ForCtorParam("FullName", opt => opt.MapFrom(src =>
                src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : "No Name"))
            .ForCtorParam("PhoneNumber", opt => opt.MapFrom(src => src.PhoneNumber))
            .ForCtorParam("FarmDescription", opt => opt.MapFrom(src => src.FarmDescription))
            .ForCtorParam("Location", opt => opt.MapFrom(src => src.Location))
            .ForCtorParam("Email", opt => opt.MapFrom(src =>
                src.User != null ? src.User.Email : "No Email"))
            .ForCtorParam("PhotoString", opt => opt.MapFrom(src =>
                src.Photo != null && src.Photo.Length > 0
                    ? Convert.ToBase64String(src.Photo)
                    : null))
            .ForCtorParam("Products", opt => opt.MapFrom(src =>
                src.OwnedProducts != null && src.OwnedProducts.Any()
                    ? src.OwnedProducts.Select(product => new AllProductsDto(
                        product.Id,
                        product.Name,
                        product.Price,
                        product.StockQuantity,
                        product.ProductPhotos != null
                            ? product.ProductPhotos.Select(photo => new ProductPhotosDto(
                                photo.Id,
                                "/uploads/" + Path.GetFileName(photo.FilePath),
                                photo.Photo,
                                photo.ProductId
                                )).ToList()
                            : new List<ProductPhotosDto>()
                    )).ToList()
                    : new List<AllProductsDto>()));
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
