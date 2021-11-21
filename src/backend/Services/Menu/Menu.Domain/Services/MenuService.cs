using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core.Exceptions;
using Domain.Core.Models;
using Domain.Core.Repositories;
using Domain.Core.Repositories.Specifications;
using Menu.Domain.Models;
using Menu.Domain.Repositories.Specifications;
using Microsoft.Extensions.Logging;

namespace Menu.Domain.Services;

public class MenuService : IMenuService
{
    private readonly ILogger<MenuService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDishesService _dishesService;
    private readonly IRestaurantsService _restaurantsService;
    private readonly IRepository<MenuItem> _menuRepository;

    public MenuService(ILogger<MenuService> logger, IUnitOfWork unitOfWork, IDishesService dishesService,
        IRestaurantsService restaurantsService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _dishesService = dishesService;
        _restaurantsService = restaurantsService;
        _menuRepository = _unitOfWork.Repository<MenuItem>();
    }

    public async Task<PagedList<MenuItem>> GetMenuAsync(int pageNumber, int pageSize)
    {
        var pagedSpecification = new PagedSpecification<MenuItem>(pageNumber, pageSize);
        var menu = await _menuRepository.FindAsync(pagedSpecification);
        var totalCount = await _menuRepository.CountAsync();
        return new PagedList<MenuItem>(menu, totalCount, pageNumber, pageSize);
    }

    public async Task<MenuItem> GetMenuItemByIdAsync(Guid id)
    {
        var menuItem = await _menuRepository.FindByIdAsync(id);

        if (menuItem == null)
        {
            throw new EntityNotFoundException(nameof(MenuItem));
        }

        return menuItem;
    }

    public async Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem)
    {
        if (menuItem.PriceRubles <= 0)
        {
            throw new ArgumentException($"{nameof(menuItem.PriceRubles)} must be greater than 0");
        }

        try
        {
            await _dishesService.GetDishByIdAsync(menuItem.DishId);
        }
        catch (EntityNotFoundException)
        {
            throw new ArgumentException($"{nameof(menuItem.DishId)} not exist");
        }

        try
        {
            await _restaurantsService.CheckRestaurantExistAsync(menuItem.RestaurantId);
        }
        catch (EntityNotFoundException)
        {
            throw new ArgumentException($"{nameof(menuItem.RestaurantId)} not exist");
        }

        var duplicationMenuItems =
            await _menuRepository.FindAsync(
                new MenuItemDishInRestaurantSpecification(menuItem.DishId, menuItem.RestaurantId));

        if (duplicationMenuItems.Any())
        {
            throw new EntityAlreadyExistsException();
        }

        await _menuRepository.AddAsync(menuItem);
        _unitOfWork.Complete();
        return menuItem;
    }

    public async Task<MenuItem> UpdateMenuItem(MenuItem menuItem)
    {
        if (menuItem.PriceRubles <= 0)
        {
            throw new ArgumentException($"{nameof(menuItem.PriceRubles)} must be greater than 0");
        }

        var menuWithSameId = await _menuRepository.FindByIdAsync(menuItem.Id);

        if (menuWithSameId == null)
        {
            throw new EntityNotFoundException(nameof(MenuItem));
        }

        if (menuWithSameId.RestaurantId != menuItem.RestaurantId)
        {
            _logger.LogError($"User change restaurant id from {menuWithSameId.RestaurantId} to {menuItem.RestaurantId}");
            throw new ArgumentException($"{nameof(menuItem.RestaurantId)} cannot be changed");
        }

        if (menuWithSameId.DishId != menuItem.DishId)
        {
            _logger.LogInformation($"User change dish id from {menuWithSameId.DishId} to {menuItem.DishId}");

            try
            {
                await _dishesService.GetDishByIdAsync(menuItem.DishId);
            }
            catch (EntityNotFoundException)
            {
                throw new ArgumentException($"{nameof(menuItem.DishId)} not exist");
            }

            var duplicationMenuItems =
                await _menuRepository.FindAsync(
                    new MenuItemDishInRestaurantSpecification(menuItem.DishId, menuWithSameId.RestaurantId));

            if (duplicationMenuItems.Any())
            {
                _logger.LogInformation($"Same dish id already exist in restaurant menu");
                throw new EntityAlreadyExistsException();
            }
        }

        menuWithSameId.PriceRubles = menuItem.PriceRubles;
        menuWithSameId.DishId = menuItem.DishId;

        _menuRepository.Update(menuWithSameId);
        _unitOfWork.Complete();
        return menuWithSameId;
    }

    public async Task DeleteMenuItemAsync(Guid id)
    {
        var menuItem = await _menuRepository.FindByIdAsync(id);

        if (menuItem == null)
        {
            throw new EntityNotFoundException(nameof(MenuItem));
        }

        _menuRepository.Remove(menuItem);
        _unitOfWork.Complete();
    }
}