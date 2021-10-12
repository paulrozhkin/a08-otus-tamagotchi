using AutoMapper;
using Infrastructure.Core.Messages.OrderQueueMessages;
using Web.HttpAggregator.Models.OrderQueue;
using KitchenOrderDish = Web.HttpAggregator.Models.OrderQueue.KitchenOrderDish;

namespace Web.HttpAggregator.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<KitchenOrderDish, NewOrderDishMessage>();
            CreateMap<KitchenOrderCreateRequest, NewKitchenOrderMessage>();
        }
    }
}