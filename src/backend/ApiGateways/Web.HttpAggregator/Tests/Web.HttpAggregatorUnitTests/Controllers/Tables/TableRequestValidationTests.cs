using AutoFixture;
using FluentValidation.TestHelper;
using Web.HttpAggregator.Infrastructure.Validation;
using Web.HttpAggregator.Models;
using Xunit;

namespace Web.HttpAggregatorUnitTests.Controllers.Tables
{
    public class TableRequestValidationTests
    {
        private readonly TableRequestValidation _validation;
        private readonly Fixture _fixture;

        public TableRequestValidationTests()
        {
            _validation = new TableRequestValidation();
            _fixture = new Fixture();
        }

        [Fact]
        public void TableWithoutName_Validate_Invalid()
        {
            // arrange
            var dish = _fixture.Build<TableRequest>().Without(x => x.Name).Create();

            // act
            var resultValidation = _validation.TestValidate(dish);

            // assert
            resultValidation.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void TableWithNegativeNumberOfPlaces_Validate_Invalid()
        {
            // arrange
            var dish = _fixture.Build<TableRequest>().With(x => x.NumberOfPlaces, -1).Create();

            // act
            var resultValidation = _validation.TestValidate(dish);

            // assert
            resultValidation.ShouldHaveValidationErrorFor(x => x.NumberOfPlaces);
        }

        [Fact]
        public void TableWithZeroNumberOfPlaces_Validate_Invalid()
        {
            // arrange
            var dish = _fixture.Build<TableRequest>().With(x => x.NumberOfPlaces, 0).Create();

            // act
            var resultValidation = _validation.TestValidate(dish);

            // assert
            resultValidation.ShouldHaveValidationErrorFor(x => x.NumberOfPlaces);
        }

        [Fact]
        public void TableWithPositiveNumberOfPlaces_Validate_Valid()
        {
            // arrange
            var dish = _fixture.Build<TableRequest>().With(x => x.NumberOfPlaces, 1).Create();

            // act
            var resultValidation = _validation.TestValidate(dish);

            // assert
            resultValidation.ShouldNotHaveValidationErrorFor(x => x.NumberOfPlaces);
        }
    }
}