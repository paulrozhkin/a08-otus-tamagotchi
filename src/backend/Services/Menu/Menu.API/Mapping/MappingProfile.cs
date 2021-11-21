using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Menu.Domain.Models;

namespace Menu.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Dish, DishesApi.Dish>()
                .ForMember(dest => dest.Photos, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate,
                    opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.UpdatedDate)));

            CreateMap<DishesApi.CrateDishRequest, Dish>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Dish.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Dish.Description))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Menu, opt => opt.Ignore());

            CreateMap<DishesApi.UpdateDishRequest, Dish>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Dish.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Dish.Description))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Dish.Id))
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Menu, opt => opt.Ignore());

            CreateMap<MenuItem, MenuApi.MenuItemRequest>()
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate,
                    opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.UpdatedDate)));

            CreateMap<MenuItem, MenuApi.MenuItemResponse>()
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate,
                    opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.UpdatedDate)));

            CreateMap<MenuApi.CreateMenuItemRequest, MenuItem>()
                .ForMember(dest => dest.DishId, opt => opt.MapFrom(src => src.MenuItem.DishId))
                .ForMember(dest => dest.PriceRubles, opt => opt.MapFrom(src => src.MenuItem.PriceRubles))
                .ForMember(dest => dest.RestaurantId, opt => opt.MapFrom(src => src.MenuItem.RestaurantId))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Dish, opt => opt.Ignore());

            CreateMap<MenuApi.UpdateMenuItemRequest, MenuItem>()
                .ForMember(dest => dest.DishId, opt => opt.MapFrom(src => src.MenuItem.DishId))
                .ForMember(dest => dest.PriceRubles, opt => opt.MapFrom(src => src.MenuItem.PriceRubles))
                .ForMember(dest => dest.RestaurantId, opt => opt.MapFrom(src => src.MenuItem.RestaurantId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MenuItem.Id))
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Dish, opt => opt.Ignore());
        }
    }
}