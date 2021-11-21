using System;
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

namespace Web.HttpAggregatorUnitTests.Controllers.Menu
{
    public class GetAllAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMenuService> _menuServiceMock;
        private readonly RestaurantMenuController _menuController;

        public GetAllAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _menuServiceMock = _fixture.Freeze<Mock<IMenuService>>();
            _menuController = _fixture.Build<RestaurantMenuController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task Menu_GetAllWithPagination_ReturnOkResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            var requestParameters = new QueryStringParameters()
            {
                PageNumber = 1,
                PageSize = 5
            };
            var menu = _fixture.Create<PaginationResponse<MenuItemResponse>>();

            _menuServiceMock.Setup(x =>
                    x.GetMenuAsync(restaurantId, requestParameters.PageNumber, requestParameters.PageSize))
                .Returns(Task.FromResult(menu));
            // act
            var result = await _menuController.GetMenuAsync(restaurantId, requestParameters);

            // assert
            result.Should().BeAssignableTo<OkObjectResult>().Which.Value.Should().Be(menu);
        }
    }
}