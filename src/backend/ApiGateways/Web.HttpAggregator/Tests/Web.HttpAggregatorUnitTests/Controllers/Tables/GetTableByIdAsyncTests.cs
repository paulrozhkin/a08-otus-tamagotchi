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
    public class GetTableByIdAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITablesService> _tablesServiceMock;
        private readonly RestaurantTablesController _tablesController;

        public GetTableByIdAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _tablesServiceMock = _fixture.Freeze<Mock<ITablesService>>();
            _tablesController = _fixture.Build<RestaurantTablesController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task Table_GetTableById_ReturnOkResponse()
        {
            // arrange
            var tableId = _fixture.Create<Guid>();
            var tableResponse = _fixture.Create<TableResponse>();

            _tablesServiceMock.Setup(x => x.GetTableByIdAsync(tableId))
                .Returns(Task.FromResult(tableResponse));
            // act
            var result = await _tablesController.GetTableByIdAsync(tableId);

            // assert
            result.Should().BeAssignableTo<OkObjectResult>().Which.Value.Should().Be(tableResponse);
        }

        [Fact]
        private async Task NotExistedTable_TryGetTableById_ReturnNotFoundResponse()
        {
            // arrange
            var tableId = _fixture.Create<Guid>();
            _tablesServiceMock.Setup(x => x.GetTableByIdAsync(tableId))
                .Throws(new EntityNotFoundException());

            // act
            var result = await _tablesController.GetTableByIdAsync(tableId);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(404);
        }
    }
}
