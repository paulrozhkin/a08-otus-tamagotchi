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
    public class UpdateMenuAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMenuService> _menuServiceMock;
        private readonly RestaurantMenuController _menuController;

        public UpdateMenuAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _menuServiceMock = _fixture.Freeze<Mock<IMenuService>>();
            _menuController = _fixture.Build<RestaurantMenuController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task Menu_UpdateMenu_ReturnOkResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            var menuId = _fixture.Create<Guid>();
            var menuRequest = _fixture.Create<MenuItemRequest>();
            var menuResponse = _fixture.Create<MenuItemResponse>();

            _menuServiceMock.Setup(x => x.UpdateMenuAsync(restaurantId, menuId, menuRequest))
                .Returns(Task.FromResult(menuResponse));

            // act
            var result = await _menuController.UpdateMenuAsync(restaurantId, menuId, menuRequest);

            // assert
            result.Should().BeAssignableTo<OkObjectResult>().Which.Value.Should().Be(menuResponse);
        }

        [Fact]
        private async Task MenuWithConflict_TryUpdateMenu_ReturnConflictResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            var menuId = _fixture.Create<Guid>();
            var menuRequest = _fixture.Create<MenuItemRequest>();
            _menuServiceMock.Setup(x => x.UpdateMenuAsync(restaurantId, menuId, menuRequest))
                .Throws(new EntityAlreadyExistsException());

            // act
            var result = await _menuController.UpdateMenuAsync(restaurantId, menuId, menuRequest);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(409);
        }

        [Fact]
        private async Task NotExistedMenu_TryUpdateMenu_ReturnNotFoundResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            var menuId = _fixture.Create<Guid>();
            var menuRequest = _fixture.Create<MenuItemRequest>();
            _menuServiceMock.Setup(x => x.UpdateMenuAsync(restaurantId, menuId, menuRequest))
                .Throws(new EntityNotFoundException());

            // act
            var result = await _menuController.UpdateMenuAsync(restaurantId, menuId, menuRequest);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(404);
        }
    }
}