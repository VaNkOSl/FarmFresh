﻿using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.User;

namespace FarmFresh.Mapper;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
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
    }
}
