using AutoFixture;
using FluentValidation.TestHelper;
using Web.HttpAggregator.Infrastructure.Validation;
using Web.HttpAggregator.Models;
using Xunit;

namespace Web.HttpAggregatorUnitTests.Controllers.Menu
{
    public class MenuItemRequestValidationTests
    {
        private readonly MenuItemRequestValidation _validation;
        private readonly Fixture _fixture;

        public MenuItemRequestValidationTests()
        {
            _validation = new MenuItemRequestValidation();
            _fixture = new Fixture();
        }

        [Fact]
        public void MenuWithoutDishId_Validate_Invalid()
        {
            // arrange
            var dish = _fixture.Build<MenuItemRequest>().Without(x => x.DishId).Create();

            // act
            var resultValidation = _validation.TestValidate(dish);

            // assert
            resultValidation.ShouldHaveValidationErrorFor(x => x.DishId);
        }

        [Fact]
        public void MenuWithNegativePrice_Validate_Invalid()
        {
            // arrange
            var dish = _fixture.Build<MenuItemRequest>().With(x => x.PriceRubles, -1).Create();

            // act
            var resultValidation = _validation.TestValidate(dish);

            // assert
            resultValidation.ShouldHaveValidationErrorFor(x => x.PriceRubles);
        }

        [Fact]
        public void MenuWithZeroPrice_Validate_Invalid()
        {
            // arrange
            var dish = _fixture.Build<MenuItemRequest>().With(x => x.PriceRubles, 0).Create();

            // act
            var resultValidation = _validation.TestValidate(dish);

            // assert
            resultValidation.ShouldHaveValidationErrorFor(x => x.PriceRubles);
        }

        [Fact]
        public void MenuWithPositivePrice_Validate_Valid()
        {
            // arrange
            var dish = _fixture.Build<MenuItemRequest>().With(x => x.PriceRubles, 1).Create();

            // act
            var resultValidation = _validation.TestValidate(dish);

            // assert
            resultValidation.ShouldNotHaveValidationErrorFor(x => x.PriceRubles);
        }
    }
}