﻿using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Menu.Domain.Models;

namespace Menu.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Dish, DishesApi.Dish>()
                .ForMember(dest => dest.Photos, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.UpdatedDate)));

            CreateMap<DishesApi.CrateDishRequest, Dish>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Dish.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Dish.Description))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());

            CreateMap<DishesApi.UpdateDishRequest, Dish>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Dish.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Dish.Description))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Dish.Id))
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
        }
    }
}