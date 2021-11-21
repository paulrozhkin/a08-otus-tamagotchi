using AutoMapper;
using Restaurants.API.Mapping;
using Xunit;

namespace Restaurants.UnitTests.Mapping
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