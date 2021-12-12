using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Restaurants.Domain.Models;

namespace Restaurants.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Restaurant, RestaurantsApi.Restaurant>()
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate,
                    opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.UpdatedDate)));

            CreateMap<RestaurantsApi.AddRestaurantRequest, Restaurant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Restaurant.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Restaurant.Longitude))
                .ForMember(dest => dest.IsCardPaymentPresent,
                    opt => opt.MapFrom(src => src.Restaurant.IsCardPaymentPresent))
                .ForMember(dest => dest.IsParkingPresent, opt => opt.MapFrom(src => src.Restaurant.IsParkingPresent))
                .ForMember(dest => dest.IsWiFiPresent, opt => opt.MapFrom(src => src.Restaurant.IsWiFiPresent))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Restaurant.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Restaurant.Photos));

            CreateMap<RestaurantsApi.UpdateRestaurantRequest, Restaurant>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Restaurant.Id))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Restaurant.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Restaurant.Longitude))
                .ForMember(dest => dest.IsCardPaymentPresent,
                    opt => opt.MapFrom(src => src.Restaurant.IsCardPaymentPresent))
                .ForMember(dest => dest.IsParkingPresent, opt => opt.MapFrom(src => src.Restaurant.IsParkingPresent))
                .ForMember(dest => dest.IsWiFiPresent, opt => opt.MapFrom(src => src.Restaurant.IsWiFiPresent))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Restaurant.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Restaurant.Photos));
        }
    }
}