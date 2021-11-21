using AutoMapper;
using Menu.API.Mapping;
using Xunit;

namespace Menu.UnitTests.Mapping
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