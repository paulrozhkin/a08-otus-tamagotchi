using AutoFixture;
using FluentValidation.TestHelper;
using Web.HttpAggregator.Infrastructure.Validation;
using Web.HttpAggregator.Models;
using Xunit;

namespace Web.HttpAggregatorUnitTests.Controllers.Dishes
{
    public class DishRequestValidationTests
    {
        private readonly DishRequestValidation _validation;
        private readonly Fixture _fixture;

        public DishRequestValidationTests()
        {
            _validation = new DishRequestValidation();
            _fixture = new Fixture();
        }

        [Fact]
        public void DishWithoutName_Validate_Invalid()
        {
            // arrange
            var dish = _fixture.Build<DishRequest>().Without(x => x.Name).Create();

            // act
            var resultValidation = _validation.TestValidate(dish);

            // assert
            resultValidation.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void DishWithoutDescription_Validate_Invalid()
        {
            // arrange
            var dish = _fixture.Build<DishRequest>().Without(x => x.Description).Create();

            // act
            var resultValidation = _validation.TestValidate(dish);

            // assert
            resultValidation.ShouldHaveValidationErrorFor(x => x.Description);
        }
    }
}