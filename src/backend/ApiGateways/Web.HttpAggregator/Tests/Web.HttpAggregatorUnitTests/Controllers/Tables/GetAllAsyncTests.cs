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

namespace Web.HttpAggregatorUnitTests.Controllers.Tables
{
    public class GetAllAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITablesService> _tablesServiceMock;
        private readonly RestaurantTablesController _tablesController;

        public GetAllAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _tablesServiceMock = _fixture.Freeze<Mock<ITablesService>>();
            _tablesController = _fixture.Build<RestaurantTablesController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task Table_GetAllWithPagination_ReturnOkResponse()
        {
            // arrange
            var restaurantId = _fixture.Create<Guid>();
            var requestParameters = new QueryStringParameters()
            {
                PageNumber = 1,
                PageSize = 5
            };
            var table = _fixture.Create<PaginationResponse<TableResponse>>();

            _tablesServiceMock.Setup(x =>
                    x.GetTablesAsync(restaurantId, requestParameters.PageNumber, requestParameters.PageSize))
                .Returns(Task.FromResult(table));
            // act
            var result = await _tablesController.GetTableAsync(restaurantId, requestParameters);

            // assert
            result.Should().BeAssignableTo<OkObjectResult>().Which.Value.Should().Be(table);
        }
    }
}