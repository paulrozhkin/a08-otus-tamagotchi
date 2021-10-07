using AutoMapper;
using OrderQueue.Core.Domain;
using System;

namespace OrderQueue.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<KitchenOrderStatus, Infrastructure.Core.OrderQueue.KitchenOrderStatus>();
            CreateMap<DishStatus, Infrastructure.Core.OrderQueue.DishStatus>();
            CreateMap<KitchenOrderDish, Infrastructure.Core.OrderQueue.KitchenOrderDish>();
            CreateMap<KitchenOrder, Infrastructure.Core.OrderQueue.KitchenOrder>();

            CreateMap<Infrastructure.Core.OrderQueue.NewOrderDish, KitchenOrderDish>()
                .ForMember(dest => dest.DishStatusId, opt => opt.MapFrom(src => 1));
            CreateMap<Infrastructure.Core.OrderQueue.NewKitchenOrder, KitchenOrder>()
                .ForMember(dest => dest.KitchenOrderStatusId, opt => opt.MapFrom(src => 1))
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
