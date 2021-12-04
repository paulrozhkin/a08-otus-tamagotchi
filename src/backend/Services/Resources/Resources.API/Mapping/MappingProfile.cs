using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Resources.API.Models;

namespace Resources.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ResourceMetadata, ResourcesApi.ResourceMetadata>()
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate,
                    opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.UpdatedDate)));
        }
    }
}