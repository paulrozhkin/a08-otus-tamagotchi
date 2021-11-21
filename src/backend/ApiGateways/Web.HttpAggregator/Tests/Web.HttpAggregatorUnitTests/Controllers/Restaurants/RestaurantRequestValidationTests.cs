using AutoFixture;
using FluentValidation.TestHelper;
using Web.HttpAggregator.Infrastructure.Validation;
using Web.HttpAggregator.Models;
using Xunit;

namespace Web.HttpAggregatorUnitTests.Controllers.Restaurants
{
    public class RestaurantRequestValidationTests
    {
        private readonly RestaurantRequestValidation _validation;
        private readonly Fixture _fixture;

        public RestaurantRequestValidationTests()
        {
            _validation = new RestaurantRequestValidation();
            _fixture = new Fixture();
        }

        [Fact]
        public void RestaurantWithoutName_Validate_Invalid()
        {
            // arrange
            var Restaurant = _fixture.Build<RestaurantRequest>().Without(x => x.Latitude).Create();

            // act
            var resultValidation = _validation.TestValidate(Restaurant);

            // assert
            resultValidation.ShouldHaveValidationErrorFor(x => x.Latitude);
        }

        [Fact]
        public void RestaurantWithoutDescription_Validate_Invalid()
        {
            // arrange
            var Restaurant = _fixture.Build<RestaurantRequest>().Without(x => x.Longitude).Create();

            // act
            var resultValidation = _validation.TestValidate(Restaurant);

            // assert
            resultValidation.ShouldHaveValidationErrorFor(x => x.Longitude);
        }
    }
}