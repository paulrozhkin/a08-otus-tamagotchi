using AutoMapper;
using Tables.API.Mapping;
using Xunit;

namespace Tables.UnitTests.Mapping
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