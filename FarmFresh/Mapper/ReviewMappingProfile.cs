using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Product;
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

        CreateMap<Review, ProductReviewDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
            .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => src.ReviewDate))
            .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => src.ReviewDate))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.FarmerId, opt => opt.MapFrom(src => src.Product.FarmerId))
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.Product.Farmer.User.FirstName + " " + src.Product.Farmer.User.LastName))
            .ForMember(dest => dest.Photos, opt => opt.MapFrom(src =>
             src.Product.ProductPhotos != null && src.Product.ProductPhotos.Any()
                    ? src.Product.ProductPhotos.Select(photo => new ProductPhotosDto(
                        photo.Id,
                        "/uploads/" + Path.GetFileName(photo.FilePath),
                        photo.Photo,
                        photo.ProductId
                    )).ToList()
                    : new List<ProductPhotosDto>()));

        CreateMap<Review,AllReviewDto>()
               .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("ProductName", opt => opt.MapFrom(src => src.Product.Name))
            .ForCtorParam("ProductId", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Photos", opt => opt.MapFrom(src =>
             src.Product.ProductPhotos != null && src.Product.ProductPhotos.Any()
                    ? src.Product.ProductPhotos.Select(photo => new ProductPhotosDto(
                        photo.Id,
                        "/uploads/" + Path.GetFileName(photo.FilePath),
                        photo.Photo,
                        photo.ProductId
                    )).ToList()
                    : new List<ProductPhotosDto>()));
    }
}
