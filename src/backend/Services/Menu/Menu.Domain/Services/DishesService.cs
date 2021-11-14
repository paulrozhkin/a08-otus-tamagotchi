using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core.Exceptions;
using Domain.Core.Models;
using Domain.Core.Repositories;
using Domain.Core.Repositories.Specifications;
using Menu.Domain.Models;
using Menu.Domain.Repositories.Specifications;

namespace Menu.Domain.Services;

public class DishesService : IDishesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Dish> _dishesRepository;

    public DishesService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _dishesRepository = _unitOfWork.Repository<Dish>();
    }

    public async Task<PagedList<Dish>> GetDishesAsync(int pageNumber, int pageSize)
    {
        var pagedSpecification = new PagedSpecification<Dish>(pageNumber, pageSize);
        var dishes = await _dishesRepository.FindAsync(pagedSpecification);
        var totalCount = await _dishesRepository.CountAsync();
        return new PagedList<Dish>(dishes, totalCount, pageNumber, pageSize);
    }

    public async Task<Dish> GetDishByIdAsync(Guid id)
    {
        var dish = await _dishesRepository.FindByIdAsync(id);

        if (dish == null)
        {
            throw new EntityNotFoundException();
        }

        return dish;
    }

    public async Task<Dish> CreateDishAsync(Dish dish)
    {
        var dishWithSameNames = await _dishesRepository.FindAsync(new DishNameSpecification(dish.Name));

        if (dishWithSameNames.Any())
        {
            throw new NameAlreadyExistsException();
        }

        await _dishesRepository.AddAsync(dish);
        _unitOfWork.Complete();
        return dish;
    }

    public async Task<Dish> UpdateDish(Dish dish)
    {
        var dishWithSameId = await _dishesRepository.FindByIdAsync(dish.Id);

        if (dishWithSameId == null)
        {
            throw new EntityNotFoundException();
        }

        if (dishWithSameId.Name != dish.Name)
        {
            var dishWithSameNames = await _dishesRepository.FindAsync(new DishNameSpecification(dish.Name));

            if (dishWithSameNames.Any(x => x.Id != dish.Id))
            {
                throw new NameAlreadyExistsException();
            }
        }

        dishWithSameId.Description = dish.Description;
        dishWithSameId.Name = dish.Name;

        _dishesRepository.Update(dishWithSameId);
        _unitOfWork.Complete();
        return dish;

    }

    public async Task DeleteDishAsync(Guid id)
    {
        var dish = await _dishesRepository.FindByIdAsync(id);

        if (dish == null)
        {
            throw new EntityNotFoundException();
        }

        _dishesRepository.Remove(dish);
        _unitOfWork.Complete();
    }
}