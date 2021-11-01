using AutoMapper;
using Infrastructure.Core.Messages.OrderQueueMessages;
using Web.HttpAggregator.Models.OrderQueue;

namespace Web.HttpAggregator.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {            
            CreateMap<KitchenOrderCreateRequest, NewKitchenOrderMessage>();            
        }
    }
}