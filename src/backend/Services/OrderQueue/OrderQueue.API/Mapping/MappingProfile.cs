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
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreatedDate.DateTime));
            CreateMap<NewKitchenOrderMessage, KitchenOrder>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}