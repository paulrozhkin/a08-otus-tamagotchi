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

namespace Web.HttpAggregatorUnitTests.Controllers.Restaurants
{
    public class UpdateRestaurantAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRestaurantsService> _restaurantsServiceMock;
        private readonly RestaurantsController _restaurantsController;

        public UpdateRestaurantAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _restaurantsServiceMock = _fixture.Freeze<Mock<IRestaurantsService>>();
            _restaurantsController = _fixture.Build<RestaurantsController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task Restaurant_UpdateRestaurant_ReturnOkResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            var restaurantRequest = _fixture.Create<RestaurantRequest>();
            var restaurantResponse = _fixture.Create<RestaurantResponse>();

            _restaurantsServiceMock.Setup(x => x.UpdateRestaurantAsync(restaurantId, restaurantRequest))
                .Returns(Task.FromResult(restaurantResponse));

            // act
            var result = await _restaurantsController.UpdateRestaurantAsync(restaurantId, restaurantRequest);

            // assert
            result.Should().BeAssignableTo<OkObjectResult>().Which.Value.Should().Be(restaurantResponse);
        }

        [Fact]
        private async Task RestaurantWithDuplicationLocation_TryUpdateRestaurant_ReturnConflictResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            var restaurantRequest = _fixture.Create<RestaurantRequest>();
            _restaurantsServiceMock.Setup(x => x.UpdateRestaurantAsync(restaurantId, restaurantRequest))
                .Throws(new EntityAlreadyExistsException());

            // act
            var result = await _restaurantsController.UpdateRestaurantAsync(restaurantId, restaurantRequest);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(409);
        }

        [Fact]
        private async Task NotExistedRestaurant_TryUpdateRestaurant_ReturnNotFoundResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            var restaurantRequest = _fixture.Create<RestaurantRequest>();
            _restaurantsServiceMock.Setup(x => x.UpdateRestaurantAsync(restaurantId, restaurantRequest))
                .Throws(new EntityNotFoundException());

            // act
            var result = await _restaurantsController.UpdateRestaurantAsync(restaurantId, restaurantRequest);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(404);
        }
    }
}