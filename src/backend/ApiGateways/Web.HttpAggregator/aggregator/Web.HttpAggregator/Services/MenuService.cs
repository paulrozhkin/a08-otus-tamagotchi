using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Core.Exceptions;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services;

public class MenuService : IMenuService
{
    private readonly IRestaurantsService _restaurantsService;

    public MenuService(IRestaurantsService restaurantsService)
    {
        _restaurantsService = restaurantsService;
    }

    public Task<PaginationResponse<MenuItemResponse>> GetMenuAsync(Guid restaurantId, int pageNumber, int pageSize)
    {
        if (_restaurantsService.GetRestaurantByIdAsync(restaurantId) == null)
        {
            throw new ArgumentException("");
        }

        return Task.FromResult(new PaginationResponse<MenuItemResponse>()
        {
            CurrentPage = pageNumber,
            Items = new List<MenuItemResponse>(),
            PageSize = pageSize,
            TotalCount = 0
        });
    }

    public Task<MenuItemResponse> GetMenuByIdAsync(Guid menuItemId)
    {
        return Task.FromResult(new MenuItemResponse() {Id = menuItemId});
    }

    public Task<MenuItemResponse> CreateMenuAsync(Guid restaurantId, MenuItemRequest menu)
    {
        return Task.FromResult(new MenuItemResponse() {Id = Guid.NewGuid()});
    }

    public Task<MenuItemResponse> UpdateMenu(Guid menuItemId, MenuItemRequest menu)
    {
        return Task.FromResult(new MenuItemResponse() {Id = menuItemId});
    }

    public Task DeleteMenuAsync(Guid menuItemId)
    {
        return Task.CompletedTask;
    }
}