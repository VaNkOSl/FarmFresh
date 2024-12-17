using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Order;
using FarmFresh.ViewModels.Product;

namespace FarmFresh.Mapper;

public class CartItemMappingProfile : Profile
{
    public CartItemMappingProfile()
    {
        CreateMap<CartItem, CartItemViewModel>()
            .ForCtorParam("ProductId", opt => opt.MapFrom(src => src.ProductId))
            .ForCtorParam("ProductName", opt => opt.MapFrom(src => src.Product.Name))
            .ForCtorParam("Quantity", opt => opt.MapFrom(src => src.Quantity))
            .ForCtorParam("Price", opt => opt.MapFrom(src => src.Product.Price))
            .ForCtorParam("TotalPrice", opt => opt.MapFrom(src => src.Quantity * src.Product.Price))
            .ForCtorParam("Photos", opt => opt.MapFrom(src =>
                src.Product.ProductPhotos.Select(photo => new ProductPhotosDto(
                    photo.Id,
                    "/uploads/" + Path.GetFileName(photo.FilePath),
                    photo.Photo,
                    photo.ProductId
                )).ToList()
            ));

    }
}
