using AutoMapper;
using Web.HttpAggregator.Models.OrderQueue;

namespace Web.HttpAggregator.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<KitchenOrderDish, ExchangeModels.OrderQueue.NewOrderDish>();
            CreateMap<KitchenOrderCreateRequest, ExchangeModels.OrderQueue.NewKitchenOrder>();
        }
    }
}
