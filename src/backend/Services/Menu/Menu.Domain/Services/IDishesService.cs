using System;
using System.Threading.Tasks;
using Domain.Core.Models;
using Domain.Core.Repositories;
using Domain.Core.Repositories.Specifications;
using Menu.Domain.Models;

namespace Menu.Domain.Services
{
    public interface IDishesService
    {
        public Task<PagedList<Dish>> GetDishesAsync(int pageNumber, int pageSize);

        public Task<Dish> GetDishByIdAsync(Guid id);

        public Task<Dish> CreateDishAsync(Dish dish);

        public Task<Dish> UpdateDish(Guid id, Dish dish);

        public Task DeleteDishAsync(Guid id);
    }

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

        public Task<Dish> GetDishByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Dish> CreateDishAsync(Dish dish)
        {
            throw new NotImplementedException();
        }

        public Task<Dish> UpdateDish(Guid id, Dish dish)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDishAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}