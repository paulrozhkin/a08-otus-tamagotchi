using AutoMapper;
using OrderQueue.Core.Domain;
using System;

namespace OrderQueue.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<KitchenOrderStatus, ExchangeModels.OrderQueue.KitchenOrderStatus>();
            CreateMap<DishStatus, ExchangeModels.OrderQueue.DishStatus>();
            CreateMap<KitchenOrderDish, ExchangeModels.OrderQueue.KitchenOrderDish>();
            CreateMap<KitchenOrder, ExchangeModels.OrderQueue.KitchenOrder>();

            CreateMap<ExchangeModels.OrderQueue.NewOrderDish, KitchenOrderDish>()
                .ForMember(dest => dest.DishStatusId, opt => opt.MapFrom(src => 1));
            CreateMap<ExchangeModels.OrderQueue.NewKitchenOrder, KitchenOrder>()
                .ForMember(dest => dest.KitchenOrderStatusId, opt => opt.MapFrom(src => 1))
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
