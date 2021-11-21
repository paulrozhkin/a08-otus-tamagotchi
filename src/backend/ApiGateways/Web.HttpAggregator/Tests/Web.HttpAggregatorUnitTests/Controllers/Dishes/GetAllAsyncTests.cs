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

namespace Web.HttpAggregatorUnitTests.Controllers.Dishes
{
    public class GetAllAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IDishesService> _dishesServiceMock;
        private readonly DishesController _dishesController;

        public GetAllAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _dishesServiceMock = _fixture.Freeze<Mock<IDishesService>>();
            _dishesController = _fixture.Build<DishesController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task Dishes_GetAllWithPagination_ReturnOkResponse()
        {
            // arrange
            var requestParameters = new QueryStringParameters()
            {
                PageNumber = 1,
                PageSize = 5
            };
            var dishes = _fixture.Create<PaginationResponse<DishResponse>>();

            _dishesServiceMock.Setup(x => x.GetDishesAsync(requestParameters.PageNumber, requestParameters.PageSize))
                .Returns(Task.FromResult(dishes));
            // act
            var result = await _dishesController.GetDishesAsync(requestParameters);

            // assert
            result.Should().BeAssignableTo<OkObjectResult>().Which.Value.Should().Be(dishes);
        }
    }
}