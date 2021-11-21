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

namespace Web.HttpAggregatorUnitTests.Controllers.Menu
{
    public class GetMenuByIdAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMenuService> _menuServiceMock;
        private readonly RestaurantMenuController _menuController;

        public GetMenuByIdAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _menuServiceMock = _fixture.Freeze<Mock<IMenuService>>();
            _menuController = _fixture.Build<RestaurantMenuController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task Menu_GetMenuById_ReturnOkResponse()
        {
            // arrange
            var menuId = _fixture.Create<Guid>();
            var menuResponse = _fixture.Create<MenuItemResponse>();

            _menuServiceMock.Setup(x => x.GetMenuByIdAsync(menuId))
                .Returns(Task.FromResult(menuResponse));
            // act
            var result = await _menuController.GetMenuByIdAsync(menuId);

            // assert
            result.Should().BeAssignableTo<OkObjectResult>().Which.Value.Should().Be(menuResponse);
        }

        [Fact]
        private async Task NotExistedMenu_TryGetMenuById_ReturnNotFoundResponse()
        {
            // arrange
            var menuId = _fixture.Create<Guid>();
            _menuServiceMock.Setup(x => x.GetMenuByIdAsync(menuId))
                .Throws(new EntityNotFoundException());

            // act
            var result = await _menuController.GetMenuByIdAsync(menuId);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(404);
        }
    }
}
