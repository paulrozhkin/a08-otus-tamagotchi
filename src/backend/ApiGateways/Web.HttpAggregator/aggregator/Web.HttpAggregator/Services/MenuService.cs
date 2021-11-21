using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DishesApi;
using Domain.Core.Exceptions;
using Grpc.Core;
using Infrastructure.Core.Localization;
using MenuApi;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services;

public class MenuService : IMenuService
{
    private readonly Menu.MenuClient _menuClient;
    private readonly IMapper _mapper;

    public MenuService(Menu.MenuClient menuClient, IMapper mapper)
    {
        _menuClient = menuClient;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<Models.MenuItemResponse>> GetMenuAsync(Guid restaurantId, int pageNumber,
        int pageSize)
    {
        var menuResponse = await _menuClient.GetMenuAsync(new GetMenuRequest()
            {PageNumber = pageNumber, PageSize = pageSize, RestaurantId = restaurantId.ToString()});

        return _mapper.Map<PaginationResponse<Models.MenuItemResponse>>(menuResponse);
    }

    public async Task<Models.MenuItemResponse> GetMenuByIdAsync(Guid menuItemId)
    {
        try
        {
            var menuItemResponse =
                await _menuClient.GetMenuItemAsync(new GetMenuItemRequest() {Id = menuItemId.ToString()});
            return _mapper.Map<Models.MenuItemResponse>(menuItemResponse.MenuItem);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found, menuItemId));
        }
    }

    public async Task<Models.MenuItemResponse> CreateMenuAsync(Guid restaurantId, Models.MenuItemRequest menu)
    {
        try
        {
            var menuItem = _mapper.Map<MenuApi.MenuItemRequest>(menu);
            menuItem.RestaurantId = restaurantId.ToString();

            var dishResponse = await _menuClient.CreateMenuItemAsync(new CreateMenuItemRequest()
            {
                MenuItem = menuItem
            });

            return _mapper.Map<Models.MenuItemResponse>(dishResponse.MenuItem);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.InvalidArgument)
        {
            throw new ArgumentException(ex.Message);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
        {
            throw new EntityAlreadyExistsException(Errors.Entities_Entity_already_exits);
        }
    }

    public async Task<Models.MenuItemResponse> UpdateMenuAsync(Guid restaurantId, Guid menuItemId, Models.MenuItemRequest menu)
    {
        try
        {
            var menuItemForRequest = _mapper.Map<MenuApi.MenuItemRequest>(menu);
            menuItemForRequest.Id = menuItemId.ToString();
            menuItemForRequest.RestaurantId = restaurantId.ToString();

            var menuItemResponse = await _menuClient.UpdateMenuItemAsync(new UpdateMenuItemRequest()
            {
                MenuItem = menuItemForRequest
            });

            return _mapper.Map<Models.MenuItemResponse>(menuItemResponse.MenuItem);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.InvalidArgument)
        {
            throw new ArgumentException(ex.Message);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found, menuItemId));
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
        {
            throw new EntityAlreadyExistsException(Errors.Entities_Entity_already_exits);
        }
    }

    public async Task DeleteMenuAsync(Guid menuItemId)
    {
        try
        {
            await _menuClient.DeleteMenuItemAsync(new DeleteMenuItemRequest() {Id = menuItemId.ToString()});
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found, menuItemId));
        }
    }
}