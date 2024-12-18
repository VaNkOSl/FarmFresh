using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Farmer;

namespace FarmFresh.Mapper;

public class FarmerLocationMappingProfile : Profile
{
    public FarmerLocationMappingProfile()
    {
        CreateMap<FarmerCreateLocationDto, FarmerLocation>()
                  .ForMember(dest => dest.FarmerId, opt => opt.MapFrom(src => src.FarmerId));

        CreateMap<FarmerUpdateLocationDto, FarmerLocation>()
            .ForMember(dest => dest.FarmerId, opt => opt.MapFrom(src => src.FarmerId));
    }
}
