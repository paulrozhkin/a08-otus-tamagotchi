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

namespace Web.HttpAggregatorUnitTests.Controllers.Tables
{
    public class DeleteTableAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITablesService> _tablesServiceMock;
        private readonly RestaurantTablesController _tablesController;

        public DeleteTableAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _tablesServiceMock = _fixture.Freeze<Mock<ITablesService>>();
            _tablesController = _fixture.Build<RestaurantTablesController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task Table_DeleteTable_ReturnOkResponse()
        {
            // arrange
            var tableItemId = _fixture.Create<Guid>();

            // act
            var result = await _tablesController.DeleteTableAsync(tableItemId);

            // assert
            result.Should().BeAssignableTo<OkResult>();
        }

        [Fact]
        private async Task NotExistedTable_TryDeleteTable_ReturnNotFoundResponse()
        {
            // arrange
            var tableItemId = _fixture.Create<Guid>();
            _tablesServiceMock.Setup(x => x.DeleteTableAsync(tableItemId))
                .Throws(new EntityNotFoundException());

            // act
            var result = await _tablesController.DeleteTableAsync(tableItemId);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(404);
        }
    }
}
