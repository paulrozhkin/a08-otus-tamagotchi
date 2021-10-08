using AutoMapper;
using Infrastructure.Core.OrderQueue;

namespace Orders.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookRestauranRequest, NewKitchenOrder>()
                .ForMember(dst => dst.Dishes, opt => opt.Ignore())
                .ForMember(dst => dst.OrderId, opt => opt.Ignore())
                .ForMember(dst => dst.RestaurantId, opt => opt.MapFrom(src => src.RestaurantId));
        }
    }
}
