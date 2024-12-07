using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Categories;

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

        CreateMap<Category, AllCategoriesDTO>()
         .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count()));
         //.ConstructUsing(src => new AllCategoriesDTO(src.Id, src.Name, src.Products.Count()));
    }
}
