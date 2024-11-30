using AutoMapper;
using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Category;
using FarmFresh.ViewModels.Farmer;
using FarmFresh.ViewModels.User;

namespace FarmFresh.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LoginViewModel, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserNameOrEmail))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserNameOrEmail));

        CreateMap<RegisterViewModel, ApplicationUser>()
            .ForMember(dest => dest.CreatedAt, opt =>
                opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.NormalizedEmail, opt =>
                opt.MapFrom(src => src.Email.ToUpper()))
            .ForMember(dest => dest.NormalizedUserName, opt =>
                opt.MapFrom(src => src.UserName.ToUpper()))
            .ForMember(dest => dest.SecurityStamp, opt =>
                opt.MapFrom(src => Guid.NewGuid().ToString().ToUpper()));

        CreateMap<ApplicationUser, ProfileViewModel>();

        CreateMap<FarmerCreateForm, Farmer>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => true))
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

        CreateMap<FarmerLocationDto, FarmerLocation>()
            .ForMember(dest => dest.FarmerId, opt => opt.Ignore());

        CreateMap<CategoryCreateForm, Category>();
        CreateMap<Category, CategoryUpdateForm>()
             .ReverseMap();
        CreateMap<CategoryUpdateForm, Category>()
             .ForMember(dest => dest.Id, opt => opt.Ignore()) 
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }

    private byte[] ConvertToByteArray(IFormFile file)
    {
        using(var memoryStream =  new MemoryStream())
        {
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
