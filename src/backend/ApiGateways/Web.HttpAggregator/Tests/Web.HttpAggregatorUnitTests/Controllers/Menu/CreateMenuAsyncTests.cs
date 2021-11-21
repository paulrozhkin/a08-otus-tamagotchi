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
    public class CreateMenuAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMenuService> _menuServiceMock;
        private readonly RestaurantMenuController _menuController;

        public CreateMenuAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _menuServiceMock = _fixture.Freeze<Mock<IMenuService>>();
            _menuController = _fixture.Build<RestaurantMenuController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task NewMenuItem_CreateMenuItem_ReturnOkResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            var newMenuItem = _fixture.Create<MenuItemRequest>();
            var menuItemResponse = _fixture.Create<MenuItemResponse>();
            _menuServiceMock.Setup(x => x.CreateMenuAsync(restaurantId, newMenuItem))
                .Returns(Task.FromResult(menuItemResponse));

            // act
            var result = await _menuController.CreateMenuAsync(restaurantId, newMenuItem);

            // assert
            result.Should().BeAssignableTo<CreatedAtActionResult>();
        }

        [Fact]
        private async Task MenuItemWithWithConflict_TryCreateMenuItem_ReturnConflictResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            var newMenuItem = _fixture.Create<MenuItemRequest>();
            _menuServiceMock.Setup(x => x.CreateMenuAsync(restaurantId, newMenuItem))
                .Throws(new EntityAlreadyExistsException());

            // act
            var result = await _menuController.CreateMenuAsync(restaurantId, newMenuItem);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(409);
        }
    }
}