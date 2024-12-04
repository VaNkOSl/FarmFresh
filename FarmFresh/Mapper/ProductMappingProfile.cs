using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Product;

namespace FarmFresh.Mapper;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.FarmerId, opt => opt.Ignore())
            .ForMember(dest => dest.Photo, opt => 
                       opt.MapFrom(src => src.PhotoFile != null 
                       ? ConvertToByteArray(src.PhotoFile) : new byte[0]));
           
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
