using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Tables.Domain.Models;

namespace Tables.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Table, TablesApi.Table>()
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate,
                    opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.UpdatedDate)));

            CreateMap<TablesApi.CreateTableRequest, Table>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Table.Name))
                .ForMember(dest => dest.NumberOfPlaces, opt => opt.MapFrom(src => src.Table.NumberOfPlaces))
                .ForMember(dest => dest.RestaurantId, opt => opt.MapFrom(src => src.Table.RestaurantId))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());

            CreateMap<TablesApi.UpdateTableRequest, Table>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Table.Name))
                .ForMember(dest => dest.NumberOfPlaces, opt => opt.MapFrom(src => src.Table.NumberOfPlaces))
                .ForMember(dest => dest.RestaurantId, opt => opt.MapFrom(src => src.Table.RestaurantId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Table.Id))
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
        }
    }
}