using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Core.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.HttpAggregator.Controllers;
using Web.HttpAggregator.Services;
using Xunit;

namespace Web.HttpAggregatorUnitTests.Controllers.Dishes
{
    public class DeleteDishAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IDishesService> _dishesServiceMock;
        private readonly DishesController _dishesController;

        public DeleteDishAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _dishesServiceMock = _fixture.Freeze<Mock<IDishesService>>();
            _dishesController = _fixture.Build<DishesController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task Dish_DeleteDish_ReturnOkResponse()
        {
            // arrange
            var dishId = _fixture.Create<Guid>();

            // act
            var result = await _dishesController.DeleteDishAsync(dishId);

            // assert
            result.Should().BeAssignableTo<OkResult>();
        }

        [Fact]
        private async Task NotExistedDish_TryDeleteDish_ReturnNotFoundResponse()
        {
            // arrange
            var dishId = _fixture.Create<Guid>();
            _dishesServiceMock.Setup(x => x.DeleteDishAsync(dishId))
                .Throws(new EntityNotFoundException());

            // act
            var result = await _dishesController.DeleteDishAsync(dishId);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(404);
        }
    }
}
