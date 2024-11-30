using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Category;

namespace FarmFresh.Mapper;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<CategoryCreateForm, Category>();
        CreateMap<Category, CategoryUpdateForm>()
            .ReverseMap();
        CreateMap<CategoryUpdateForm, Category>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}
