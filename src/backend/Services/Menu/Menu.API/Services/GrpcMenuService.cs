using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Infrastructure.Core.Localization;
using Menu.Domain.Services;
using MenuApi;
using Microsoft.Extensions.Logging;

namespace Menu.API.Services
{
    public class GrpcMenuService : MenuApi.Menu.MenuBase
    {
        private readonly ILogger<GrpcMenuService> _logger;
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;

        public GrpcMenuService(ILogger<GrpcMenuService> logger,
            IMenuService menuItemsService,
            IMapper mapper)
        {
            _logger = logger;
            _menuService = menuItemsService;
            _mapper = mapper;
        }

        public override async Task<GetMenuResponse> GetMenu(GetMenuRequest request, ServerCallContext context)
        {
            var menu = await _menuService.GetMenuAsync(request.PageNumber, request.PageSize);

            var menuResponse = new GetMenuResponse()
            {
                CurrentPage = menu.CurrentPage,
                PageSize = menu.PageSize,
                TotalCount = menu.TotalCount
            };

            var menuDto = _mapper.Map<List<MenuItem>>(menu);

            menuResponse.MenuItems.AddRange(menuDto);

            return menuResponse;
        }

        public override async Task<GetMenuItemResponse> GetMenuItem(GetMenuItemRequest request,
            ServerCallContext context)
        {
            try
            {
                var menuItem = await _menuService.GetMenuItemByIdAsync(Guid.Parse(request.Id));
                var menuItemDto = _mapper.Map<MenuItem>(menuItem);

                var menuItemResponse = new GetMenuItemResponse()
                {
                    MenuItem = menuItemDto
                };

                return menuItemResponse;
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, Menu {request.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }

        public override async Task<CrateMenuItemResponse> CrateMenuItem(CrateMenuItemRequest request,
            ServerCallContext context)
        {
            var menuItemModel = _mapper.Map<Domain.Models.MenuItem>(request);

            try
            {
                var createdMenuItem = await _menuService.CreateMenuItemAsync(menuItemModel);

                return new CrateMenuItemResponse()
                {
                    MenuItem = _mapper.Map<MenuItem>(createdMenuItem)
                };
            }
            catch (EntityAlreadyExistsException)
            {
                _logger.LogError(string.Format(Errors.Menu_MenuItem_for_dish__0__already_exist, menuItemModel.DishId));
                throw new RpcException(new Status(StatusCode.AlreadyExists, Errors.Entities_Entity_already_exits));
            }
        }

        public override async Task<UpdateMenuItemResponse> UpdateMenuItem(UpdateMenuItemRequest request,
            ServerCallContext context)
        {
            var menuItemModel = _mapper.Map<Domain.Models.MenuItem>(request);

            try
            {
                var updateMenuItem = await _menuService.UpdateMenuItem(menuItemModel);

                return new UpdateMenuItemResponse()
                {
                    MenuItem = _mapper.Map<MenuItem>(updateMenuItem)
                };
            }
            catch (EntityAlreadyExistsException)
            {
                _logger.LogError(string.Format(Errors.Menu_MenuItem_for_dish__0__already_exist, menuItemModel.DishId));
                throw new RpcException(new Status(StatusCode.AlreadyExists, Errors.Entities_Entity_already_exits));
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, Dish {menuItemModel.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }

        public override async Task<Empty> DeleteMenuItem(DeleteMenuItemRequest request, ServerCallContext context)
        {
            var idForDelete = request.Id;

            try
            {
                await _menuService.DeleteMenuItemAsync(Guid.Parse(idForDelete));
                return new Empty();
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, MenuItem {idForDelete}");
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }
    }
}