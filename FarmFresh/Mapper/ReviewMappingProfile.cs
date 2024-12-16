using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Review;

namespace FarmFresh.Mapper;

public class ReviewMappingProfile : Profile
{
    public ReviewMappingProfile()
    {
        CreateMap<ProductReviewCreateDto, Review>()
            .ForMember(dest => dest.ProductId, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<ProductReviewUpdateDto, Review>()
            .ForMember(dest => dest.ProductId, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
             .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => DateTime.Now))
             .ReverseMap();
    }
}
