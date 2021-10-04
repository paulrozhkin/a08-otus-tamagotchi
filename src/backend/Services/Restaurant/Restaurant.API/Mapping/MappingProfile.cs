using AutoMapper;
using Restaurant.API.Models;
using Restaurant.API.Models.Dish;
using Restaurant.API.Models.KitchenOrder;
using Restaurant.Core.Domain;

namespace Restaurant.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<KitchenOrderStatus, KitchenOrderStatusResponse>();
            CreateMap<DishStatus, DishStatusResponse>();
            CreateMap<KitchenOrderDish, DishResponse>();
            CreateMap<KitchenOrder, KitchenOrderResponse>();
            CreateMap<DishUpdateRequest, KitchenOrderDish>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DishStatusId, opt => opt.MapFrom(src => src.StatusId));
        }
    }
}
