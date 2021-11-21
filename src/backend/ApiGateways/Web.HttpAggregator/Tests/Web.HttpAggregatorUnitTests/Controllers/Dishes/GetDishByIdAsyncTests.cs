using System;
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
    public class GetDishByIdAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IDishesService> _dishesServiceMock;
        private readonly DishesController _dishesController;

        public GetDishByIdAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _dishesServiceMock = _fixture.Freeze<Mock<IDishesService>>();
            _dishesController = _fixture.Build<DishesController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task Dish_GetDishById_ReturnOkResponse()
        {
            // arrange
            var dishId = _fixture.Create<Guid>();
            var dishResponse = _fixture.Create<DishResponse>();

            _dishesServiceMock.Setup(x => x.GetDishByIdAsync(dishId))
                .Returns(Task.FromResult(dishResponse));
            // act
            var result = await _dishesController.GetDishByIdAsync(dishId);

            // assert
            result.Should().BeAssignableTo<OkObjectResult>().Which.Value.Should().Be(dishResponse);
        }

        [Fact]
        private async Task NotExistedDish_TryGetDishById_ReturnNotFoundResponse()
        {
            // arrange
            var dishId = _fixture.Create<Guid>();
            _dishesServiceMock.Setup(x => x.GetDishByIdAsync(dishId))
                .Throws(new EntityNotFoundException());

            // act
            var result = await _dishesController.GetDishByIdAsync(dishId);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(404);
        }
    }
}
