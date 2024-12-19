﻿using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Order;
using FarmFresh.ViewModels.Product;

namespace FarmFresh.Mapper;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<OrderProduct, OrderListViewModel>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("OrderId", opt => opt.MapFrom(src => src.OrderId))
            .ForCtorParam("ProductName", opt => opt.MapFrom(src => src.Product.Name))
            .ForCtorParam("OrderStatus", opt => opt.MapFrom(src => src.Order.OrderStatus.ToString()))
            .ForCtorParam("Price", opt => opt.MapFrom(src => src.Price))
            .ForCtorParam("Quantity", opt => opt.MapFrom(src => src.Quantity))
            .ForCtorParam("Photos", opt => opt.MapFrom(src =>
        src.Product.ProductPhotos.Select(photo => new ProductPhotosDto(
            photo.Id,
            "/uploads/" + Path.GetFileName(photo.FilePath),
            photo.Photo,
            photo.ProductId
        )).ToList()));

        CreateMap<OrderProduct, OrderDetailsViewModel>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("OrderId", opt => opt.MapFrom(src => src.Order.Id))
            .ForCtorParam("Quantity", opt => opt.MapFrom(src => src.Quantity))
            .ForCtorParam("Price", opt => opt.MapFrom(src => src.Price))
            .ForCtorParam("FirstName", opt => opt.MapFrom(src => src.Order.FirstName))
            .ForCtorParam("LastName", opt => opt.MapFrom(src => src.Order.LastName))
            .ForCtorParam("Adress", opt => opt.MapFrom(src => src.Order.Adress))
            .ForCtorParam("PhoneNumber", opt => opt.MapFrom(src => src.Order.PhoneNumber))
            .ForCtorParam("Email", opt => opt.MapFrom(src => src.Order.Email))
            .ForCtorParam("ProductName", opt => opt.MapFrom(src => src.Product.Name))
            .ForCtorParam("CreatedDate", opt => opt.MapFrom(src => src.Order.CreateOrderdDate))
            .ForCtorParam("DeliveryOption", opt => opt.MapFrom(src => src.Order.DeliveryOption))
            .ForCtorParam("OrderStatus", opt => opt.MapFrom(src => src.Order.OrderStatus))
            .ForCtorParam("ProductDescription", opt => opt.MapFrom(src => src.Product.Description))
            .ForCtorParam("Origin", opt => opt.MapFrom(src => src.Product.Origin))
            .ForCtorParam("FarmerName", opt => opt.MapFrom(src => src.Product.Farmer.User.FirstName + " " + src.Product.Farmer.User.LastName))
            .ForCtorParam("ProductPrice", opt => opt.MapFrom(src => src.Product.Price))
            .ForCtorParam("Seasons", opt => opt.MapFrom(src => src.Product.SuitableSeason))
            .ForCtorParam("HarvestDate", opt => opt.MapFrom(src => src.Product.HarvestDate))
            .ForCtorParam("ExpirationDate", opt => opt.MapFrom(src => src.Product.ExpirationDate))
            .ForCtorParam("Photos", opt => opt.MapFrom(src =>
                src.Order.ProductPhotos.Select(photo => new ProductPhotosDto(
                    photo.Id,
                    "/uploads/" + Path.GetFileName(photo.FilePath),
                    photo.Photo,
                    photo.ProductId
                )).ToList()));

        CreateMap<OrderProduct, FarmerOrderListViewModel>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("ProductName", opt => opt.MapFrom(src => src.Product.Name))
            .ForCtorParam("UserFullName", opt => opt.MapFrom(src => src.Product.Farmer.User.FirstName + " " + src.Product.Farmer.User.LastName))
			.ForCtorParam("Price", opt => opt.MapFrom(src => src.Price))
			.ForCtorParam("Quantity", opt => opt.MapFrom(src => src.Quantity))
			 .ForCtorParam("Photos", opt => opt.MapFrom(src =>
		src.Product.ProductPhotos.FirstOrDefault() == null ? null : new ProductPhotosDto(
			src.Product.ProductPhotos.FirstOrDefault().Id,
			"/uploads/" + Path.GetFileName(src.Product.ProductPhotos.FirstOrDefault().FilePath),
			src.Product.ProductPhotos.FirstOrDefault().Photo,
			src.Product.Id
		)
	));

		CreateMap<CreateOrderDto, Order>()
         .ForMember(dest => dest.Id, opt => opt.Ignore())
         .ForMember(dest => dest.UserId, opt => opt.Ignore())
         .ForMember(dest => dest.OrderProducts, opt => opt.Ignore())
         .ForMember(dest => dest.CreateOrderdDate, opt => opt.MapFrom(src => DateTime.Now))
         .ForMember(dest => dest.ProductPhotos, opt => opt.Ignore());

    }
}