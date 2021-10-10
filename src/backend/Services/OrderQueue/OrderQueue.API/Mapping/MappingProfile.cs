using AutoMapper;
using OrderQueue.Core.Domain;
using System;
using OrderMessages = Infrastructure.Core.Messages.OrderQueue;

namespace OrderQueue.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<KitchenOrderStatus, OrderMessages.KitchenOrderStatus>();
            CreateMap<DishStatus, OrderMessages.DishStatus>();
            CreateMap<KitchenOrderDish, OrderMessages.KitchenOrderDish>();
            CreateMap<KitchenOrder, OrderMessages.KitchenOrder>();

            CreateMap<OrderMessages.NewOrderDish, KitchenOrderDish>();
            CreateMap<OrderMessages.NewKitchenOrder, KitchenOrder>()
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}