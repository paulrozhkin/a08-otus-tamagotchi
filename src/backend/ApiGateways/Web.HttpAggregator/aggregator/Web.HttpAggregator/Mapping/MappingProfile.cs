using System.Collections.Generic;
using AutoMapper;
using DishesApi;
using Google.Protobuf.Collections;
using Infrastructure.Core.Messages.OrderQueueMessages;
using Web.HttpAggregator.Models;
using Web.HttpAggregator.Models.OrderQueue;

namespace Web.HttpAggregator.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<KitchenOrderCreateRequest, NewKitchenOrderMessage>();

            CreateMap<Dish, DishResponse>();
            CreateMap<DishRequest, Dish>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
            CreateMap<GetDishesResponse, PaginationResponse<DishResponse>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Dishes));
        }
    }
}