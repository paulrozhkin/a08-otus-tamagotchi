﻿using System;
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

namespace Web.HttpAggregatorUnitTests.Controllers.Dishes
{
    public class UpdateDishAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IDishesService> _dishesServiceMock;
        private readonly DishesController _dishesController;

        public UpdateDishAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _dishesServiceMock = _fixture.Freeze<Mock<IDishesService>>();
            _dishesController = _fixture.Build<DishesController>().OmitAutoProperties().Create();
        }

        [Fact]
        private async Task Dish_UpdateDish_ReturnOkResponse()
        {
            // arrange
            var dishId = _fixture.Create<Guid>();
            var dishRequest = _fixture.Create<DishRequest>();
            var dishResponse = _fixture.Create<DishResponse>();

            _dishesServiceMock.Setup(x => x.UpdateDish(dishId, dishRequest))
                .Returns(Task.FromResult(dishResponse));

            // act
            var result = await _dishesController.UpdateDishAsync(dishId, dishRequest);

            // assert
            result.Should().BeAssignableTo<OkObjectResult>().Which.Value.Should().Be(dishResponse);
        }

        [Fact]
        private async Task DishWithDuplicationName_TryUpdateDish_ReturnConflictResponse()
        {
            // arrange
            var dishId = _fixture.Create<Guid>();
            var dishRequest = _fixture.Create<DishRequest>();
            _dishesServiceMock.Setup(x => x.UpdateDish(dishId, dishRequest))
                .Throws(new NameAlreadyExistsException());

            // act
            var result = await _dishesController.UpdateDishAsync(dishId, dishRequest);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(409);
        }

        [Fact]
        private async Task NotExistedDish_TryUpdateDish_ReturnNotFoundResponse()
        {
            // arrange
            var dishId = _fixture.Create<Guid>();
            var dishRequest = _fixture.Create<DishRequest>();
            _dishesServiceMock.Setup(x => x.UpdateDish(dishId, dishRequest))
                .Throws(new EntityNotFoundException());

            // act
            var result = await _dishesController.UpdateDishAsync(dishId, dishRequest);

            // assert
            result.Should().BeAssignableTo<ObjectResult>().Which.StatusCode.Should().Be(404);
        }
    }
}