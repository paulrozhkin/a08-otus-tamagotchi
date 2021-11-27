using AutoMapper;
using DishesApi;
using Infrastructure.Core.Messages.OrderQueueMessages;
using MenuApi;
using TablesApi;
using RestaurantsApi;
using Web.HttpAggregator.Models;
using Web.HttpAggregator.Models.OrderQueue;
using MenuItemResponse = Web.HttpAggregator.Models.MenuItemResponse;

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


            CreateMap<Restaurant, RestaurantResponse>();
            CreateMap<RestaurantRequest, Restaurant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
            CreateMap<GetRestaurantsResponse, PaginationResponse<RestaurantResponse>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Restaurants));

            CreateMap<MenuApi.MenuItemResponse, MenuItemResponse>();
            CreateMap<Models.MenuItemRequest, MenuApi.MenuItemRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RestaurantId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
            CreateMap<GetMenuResponse, PaginationResponse<MenuItemResponse>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.MenuItems));

            CreateMap<Table, TableResponse>();
            CreateMap<TableRequest, Table>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RestaurantId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
            CreateMap<GetTablesResponse, PaginationResponse<TableResponse>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Tables));
        }
    }
}