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

namespace Web.HttpAggregatorUnitTests.Controllers.Menu
{
    public class DeleteMenuAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMenuService> _menuServiceMock;
        private readonly RestaurantMenuController _menuController;

        public DeleteMenuAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _menuServiceMock = _fixture.Freeze<Mock<IMenuService>>();
            _menuController = _fixture.Build<RestaurantMenuController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task MenuItem_DeleteMenuItem_ReturnOkResponse()
        {
            // arrange
            var menuItemId = _fixture.Create<Guid>();

            // act
            var result = await _menuController.DeleteMenuAsync(menuItemId);

            // assert
            result.Should().BeAssignableTo<OkResult>();
        }

        [Fact]
        private async Task NotExistedMenuItem_TryDeleteMenuItem_ReturnNotFoundResponse()
        {
            // arrange
            var menuItemId = _fixture.Create<Guid>();
            _menuServiceMock.Setup(x => x.DeleteMenuAsync(menuItemId))
                .Throws(new EntityNotFoundException());

            // act
            var result = await _menuController.DeleteMenuAsync(menuItemId);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(404);
        }
    }
}
