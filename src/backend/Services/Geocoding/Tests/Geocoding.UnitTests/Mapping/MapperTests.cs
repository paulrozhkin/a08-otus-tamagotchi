using AutoMapper;
using Geocoding.API.Mapping;
using Xunit;

namespace Geocoding.UnitTests.Mapping
{
    public class MapperTests
    {
        [Fact]
        public void TestMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
        }
    }
}