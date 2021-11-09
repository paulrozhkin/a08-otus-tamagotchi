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
            CreateMap<KitchenOrder, KitchenOrderMessage>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<NewKitchenOrderMessage, KitchenOrder>()
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}