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
    public class CreateRestaurantAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRestaurantsService> _restaurantsServiceMock;
        private readonly RestaurantsController _restaurantsController;

        public CreateRestaurantAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _restaurantsServiceMock = _fixture.Freeze<Mock<IRestaurantsService>>();
            _restaurantsController = _fixture.Build<RestaurantsController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task NewRestaurant_CreateRestaurant_ReturnOkResponse()
        {
            // arrange
            var newRestaurant = _fixture.Create<RestaurantRequest>();
            var restaurantResponse = _fixture.Create<RestaurantResponse>();
            _restaurantsServiceMock.Setup(x => x.CreateRestaurantAsync(newRestaurant))
                .Returns(Task.FromResult(restaurantResponse));

            // act
            var result = await _restaurantsController.CreateRestaurantAsync(newRestaurant);

            // assert
            result.Should().BeAssignableTo<CreatedAtActionResult>();
        }

        [Fact]
        private async Task RestaurantWithDuplicationLocation_TryCreateRestaurant_ReturnConflictResponse()
        {
            // arrange
            var newRestaurant = _fixture.Create<RestaurantRequest>();
            _restaurantsServiceMock.Setup(x => x.CreateRestaurantAsync(newRestaurant))
                .Throws(new EntityAlreadyExistsException());

            // act
            var result = await _restaurantsController.CreateRestaurantAsync(newRestaurant);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(409);
        }
    }
}