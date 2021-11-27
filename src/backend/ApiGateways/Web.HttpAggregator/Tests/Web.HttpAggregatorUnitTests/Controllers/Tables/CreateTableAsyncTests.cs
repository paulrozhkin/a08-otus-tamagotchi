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
    public class CreateTableAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITablesService> _tablesServiceMock;
        private readonly RestaurantTablesController _tablesController;

        public CreateTableAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _tablesServiceMock = _fixture.Freeze<Mock<ITablesService>>();
            _tablesController = _fixture.Build<RestaurantTablesController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task NewTable_CreateTable_ReturnOkResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            var newTable = _fixture.Create<TableRequest>();
            var tableItemResponse = _fixture.Create<TableResponse>();
            _tablesServiceMock.Setup(x => x.CreateTableAsync(restaurantId, newTable))
                .Returns(Task.FromResult(tableItemResponse));

            // act
            var result = await _tablesController.CreateTableAsync(restaurantId, newTable);

            // assert
            result.Should().BeAssignableTo<CreatedAtActionResult>();
        }

        [Fact]
        private async Task TableWithWithConflict_TryCreateTable_ReturnConflictResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            var newTable = _fixture.Create<TableRequest>();
            _tablesServiceMock.Setup(x => x.CreateTableAsync(restaurantId, newTable))
                .Throws(new EntityAlreadyExistsException());

            // act
            var result = await _tablesController.CreateTableAsync(restaurantId, newTable);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(409);
        }
    }
}