using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Infrastructure.Core.Messages.OrderQueueMessages;
using Orders.Domain.Models;

namespace Orders.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrdersApi.Order>()
                .ForMember(dest => dest.VisitTime,
                    opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.VisitTime)))
                .ForMember(dest => dest.AmountRubles,
                    opt => opt.MapFrom(src => src.AmountOfRubles))
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate,
                    opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.UpdatedDate)));

            CreateMap<OrdersApi.Menu, MenuPosition>()
                .ForMember(dest => dest.MenuItemId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate,
                    opt => opt.Ignore());

            CreateMap<MenuPosition, OrdersApi.Menu>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.MenuItemId));

            CreateMap<OrdersApi.BookRestauranRequest, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RestaurantId, opt => opt.MapFrom(src => src.Order.RestaurantId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Order.Status))
                .ForMember(dest => dest.NumberOfPersons, opt => opt.MapFrom(src => src.Order.NumberOfPersons))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Order.Comment))
                .ForMember(dest => dest.VisitTime, opt => opt.MapFrom(src => src.Order.VisitTime.ToDateTimeOffset()))
                .ForMember(dest => dest.Menu, opt => opt.MapFrom(src => src.Order.Menu))
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Order.ClientId))
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.AmountOfRubles, opt => opt.Ignore());


            CreateMap<OrdersApi.BookRestauranRequest, NewKitchenOrderMessage>()
                .ForMember(dst => dst.OrderId, opt => opt.Ignore());
        }
    }
}