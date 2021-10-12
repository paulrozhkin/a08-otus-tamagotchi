using AutoMapper;
using OrderQueue.Core.Domain;
using System;
using Infrastructure.Core.Messages.OrderQueueMessages;

namespace OrderQueue.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<KitchenOrderStatus, KitchenOrderStatusMessage>();
            CreateMap<DishStatus, DishStatusMessage>();
            CreateMap<KitchenOrderDish, KitchenOrderDishMessage>();
            CreateMap<KitchenOrder, KitchenOrderMessage>();

            CreateMap<NewOrderDishMessage, KitchenOrderDish>();
            CreateMap<NewKitchenOrderMessage, KitchenOrder>()
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}