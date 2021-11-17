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

namespace Web.HttpAggregatorUnitTests.Controllers.Restaurants
{
    public class DeleteRestaurantAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRestaurantsService> _restaurantsServiceMock;
        private readonly RestaurantsController _restaurantsController;

        public DeleteRestaurantAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _restaurantsServiceMock = _fixture.Freeze<Mock<IRestaurantsService>>();
            _restaurantsController = _fixture.Build<RestaurantsController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task Restaurant_DeleteRestaurant_ReturnOkResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();

            // act
            var result = await _restaurantsController.DeleteRestaurantAsync(restaurantId);

            // assert
            result.Should().BeAssignableTo<OkResult>();
        }

        [Fact]
        private async Task NotExistedRestaurant_TryDeleteRestaurant_ReturnNotFoundResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            _restaurantsServiceMock.Setup(x => x.DeleteRestaurantAsync(restaurantId))
                .Throws(new EntityNotFoundException());

            // act
            var result = await _restaurantsController.DeleteRestaurantAsync(restaurantId);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(404);
        }
    }
}
