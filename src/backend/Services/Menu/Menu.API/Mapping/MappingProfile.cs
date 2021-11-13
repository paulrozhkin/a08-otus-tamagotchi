using AutoMapper;
using Menu.Domain.Models;

namespace Menu.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {            
            CreateMap<Dish, DishesApi.Dish>();
            CreateMap<DishesApi.Dish, Dish>();
        }
    }
}