using AutoMapper;
using Web.HttpAggregator.Mapping;
using Xunit;

namespace Web.HttpAggregatorUnitTests.Mapping
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