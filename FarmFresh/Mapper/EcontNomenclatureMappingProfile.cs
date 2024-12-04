using AutoMapper;
using FarmFresh.Data.Models.Econt.DTOs;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Enums;

namespace FarmFresh.Mapper
{
    public class EcontNomenclatureMappingProfile : Profile
    {
        public EcontNomenclatureMappingProfile()
        {
            CreateMap<Address, AddressDTO>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ReverseMap();

            CreateMap<City, CityDTO>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.ServingOffices, opt => opt.MapFrom(src => src.ServingOffices))
                .ReverseMap();

            CreateMap<Office, OfficeDTO>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ReverseMap();

            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<GeoLocation, GeoLocationDTO>().ReverseMap();
            CreateMap<Quarter, QuarterDTO>().ReverseMap();
            CreateMap<Street, StreetDTO>().ReverseMap();
            CreateMap<ServingOfficeElement, ServingOfficeElementDTO>().ReverseMap();
        }
    }
}
