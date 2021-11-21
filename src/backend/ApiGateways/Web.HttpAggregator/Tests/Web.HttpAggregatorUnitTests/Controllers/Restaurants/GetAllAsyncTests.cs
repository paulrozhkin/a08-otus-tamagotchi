using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.HttpAggregator.Controllers;
using Web.HttpAggregator.Models;
using Web.HttpAggregator.Models.QueryParameters;
using Web.HttpAggregator.Services;
using Xunit;

namespace Web.HttpAggregatorUnitTests.Controllers.Restaurants
{
    public class GetAllAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRestaurantsService> _restaurantsServiceMock;
        private readonly RestaurantsController _restaurantsController;

        public GetAllAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _restaurantsServiceMock = _fixture.Freeze<Mock<IRestaurantsService>>();
            _restaurantsController = _fixture.Build<RestaurantsController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task Restaurants_GetAllWithPagination_ReturnOkResponse()
        {
            // arrange
            var requestParameters = new RestaurantParameters()
            {
                PageNumber = 1,
                PageSize = 5,
                Address = string.Empty
            };
            var restaurants = _fixture.Create<PaginationResponse<RestaurantResponse>>();

            _restaurantsServiceMock.Setup(x =>
                    x.GetRestaurantsAsync(requestParameters.PageNumber, requestParameters.PageSize,
                        requestParameters.Address))
                .Returns(Task.FromResult(restaurants));

            // act
            var result = await _restaurantsController.GetRestaurantsAsync(requestParameters);

            // assert
            result.Should().BeAssignableTo<OkObjectResult>().Which.Value.Should().Be(restaurants);
        }
    }
}