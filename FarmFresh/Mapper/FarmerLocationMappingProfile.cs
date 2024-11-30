using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Farmer;

namespace FarmFresh.Mapper;

public class FarmerLocationMappingProfile : Profile
{
    public FarmerLocationMappingProfile()
    {
        CreateMap<FarmerLocationDto, FarmerLocation>()
            .ForMember(dest => dest.FarmerId, opt => opt.Ignore());
    }
}
