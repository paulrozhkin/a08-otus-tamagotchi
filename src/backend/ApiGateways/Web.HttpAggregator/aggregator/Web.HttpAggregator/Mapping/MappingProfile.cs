using AutoMapper;
using Web.HttpAggregator.Models.OrderQueue;
using Infrastructure.Core.OrderQueue;
using KitchenOrderDish = Web.HttpAggregator.Models.OrderQueue.KitchenOrderDish;

namespace Web.HttpAggregator.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<KitchenOrderDish, NewOrderDish>();
            CreateMap<KitchenOrderCreateRequest, NewKitchenOrder>();
        }
    }
}
