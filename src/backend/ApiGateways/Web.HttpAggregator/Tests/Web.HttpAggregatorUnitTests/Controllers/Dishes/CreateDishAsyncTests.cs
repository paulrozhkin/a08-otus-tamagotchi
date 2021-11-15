using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Core.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.HttpAggregator.Controllers;
using Web.HttpAggregator.Models;
using Web.HttpAggregator.Services;
using Xunit;

namespace Web.HttpAggregatorUnitTests.Controllers.Dishes
{
    public class CreateDishAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IDishesService> _dishesServiceMock;
        private readonly DishesController _dishesController;

        public CreateDishAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _dishesServiceMock = _fixture.Freeze<Mock<IDishesService>>();
            _dishesController = _fixture.Build<DishesController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task NewDish_CreateDish_ReturnOkResponse()
        {
            // arrange
            var newDish = _fixture.Create<DishRequest>();
            var dishResponse = _fixture.Create<DishResponse>();
            _dishesServiceMock.Setup(x => x.CreateDishAsync(newDish))
                .Returns(Task.FromResult(dishResponse));

            // act
            var result = await _dishesController.CreateDishAsync(newDish);

            // assert
            result.Should().BeAssignableTo<CreatedAtActionResult>();
        }

        [Fact]
        private async Task DishWithDuplicationName_TryCreateDish_ReturnConflictResponse()
        {
            // arrange
            var newDish = _fixture.Create<DishRequest>();
            _dishesServiceMock.Setup(x => x.CreateDishAsync(newDish))
                .Throws(new EntityAlreadyExistsException());

            // act
            var result = await _dishesController.CreateDishAsync(newDish);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(409);
        }
    }
}