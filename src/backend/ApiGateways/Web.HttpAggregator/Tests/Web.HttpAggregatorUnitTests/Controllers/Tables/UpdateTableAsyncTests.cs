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

namespace Web.HttpAggregatorUnitTests.Controllers.Tables
{
    public class UpdateTableAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITablesService> _tablesServiceMock;
        private readonly RestaurantTablesController _tablesController;

        public UpdateTableAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _tablesServiceMock = _fixture.Freeze<Mock<ITablesService>>();
            _tablesController = _fixture.Build<RestaurantTablesController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task Table_UpdateTable_ReturnOkResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            var tableId = _fixture.Create<Guid>();
            var tableRequest = _fixture.Create<TableRequest>();
            var tableResponse = _fixture.Create<TableResponse>();

            _tablesServiceMock.Setup(x => x.UpdateTableAsync(restaurantId, tableId, tableRequest))
                .Returns(Task.FromResult(tableResponse));

            // act
            var result = await _tablesController.UpdateTableAsync(restaurantId, tableId, tableRequest);

            // assert
            result.Should().BeAssignableTo<OkObjectResult>().Which.Value.Should().Be(tableResponse);
        }

        [Fact]
        private async Task TableWithConflict_TryUpdateTable_ReturnConflictResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            var tableId = _fixture.Create<Guid>();
            var tableRequest = _fixture.Create<TableRequest>();
            _tablesServiceMock.Setup(x => x.UpdateTableAsync(restaurantId, tableId, tableRequest))
                .Throws(new EntityAlreadyExistsException());

            // act
            var result = await _tablesController.UpdateTableAsync(restaurantId, tableId, tableRequest);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(409);
        }

        [Fact]
        private async Task NotExistedTable_TryUpdateTable_ReturnNotFoundResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            var tableId = _fixture.Create<Guid>();
            var tableRequest = _fixture.Create<TableRequest>();
            _tablesServiceMock.Setup(x => x.UpdateTableAsync(restaurantId, tableId, tableRequest))
                .Throws(new EntityNotFoundException());

            // act
            var result = await _tablesController.UpdateTableAsync(restaurantId, tableId, tableRequest);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(404);
        }
    }
}