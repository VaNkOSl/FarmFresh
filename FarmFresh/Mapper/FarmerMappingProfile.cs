using AutoMapper;
using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Farmer;

namespace FarmFresh.Mapper;

public class FarmerMappingProfile : Profile
{
    public FarmerMappingProfile()
    {
        CreateMap<FarmerForCreationDto, Farmer>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => true))
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
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                src.User != null ? src.User.FirstName + " " + src.User.LastName : "No Name"))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src =>
                src.PhoneNumber != null ? src.PhoneNumber : "No Phone Number"))
            .ForMember(dest => dest.PhotoString, opt => opt.MapFrom(src =>
                src.Photo != null ? Convert.ToBase64String(src.Photo) : string.Empty));

        CreateMap<(IEnumerable<FarmersViewModel> farmers, MetaData metaData, string searchTerm), FarmersListViewModel>()
          .ForMember(dest => dest.Farmers, opt => opt.MapFrom(src => src.farmers))
          .ForMember(dest => dest.MetaData, opt => opt.MapFrom(src => src.metaData))
          .ForMember(dest => dest.SearchTerm, opt => opt.MapFrom(src => src.searchTerm));
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
