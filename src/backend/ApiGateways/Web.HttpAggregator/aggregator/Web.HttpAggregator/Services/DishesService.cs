using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DishesApi;
using Domain.Core.Exceptions;
using Infrastructure.Core.Localization;
using Microsoft.Extensions.Logging;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public class DishesService : IDishesService
    {
        private readonly ILogger<DishesService> _logger;
        private readonly Dishes.DishesClient _dishesClient;

        public DishesService(ILogger<DishesService> logger, Dishes.DishesClient dishesClient)
        {
            _logger = logger;
            _dishesClient = dishesClient;
        }


        public async Task<PaginationResponse<DishResponse>> GetDishesAsync(int pageNumber, int pageSize)
        {
            var dishes = await _dishesClient.GetDishesAsync(new GetDishesRequest() {PageNumber = pageNumber, PageSize = pageSize});

            var dishesDto = dishes.Dishes.Select(x => new DishResponse()
            {
                Description = x.Description,
                Id = Guid.Parse(x.Id),
                Name = x.Name
            }).ToList();

            return new PaginationResponse<DishResponse>()
            {
                CurrentPage = dishes.CurrentPage,
                PageSize = dishes.PageSize,
                TotalCount = dishes.TotalCount,
                Items = dishesDto
            };
        }

        public Task<DishResponse> GetDishByIdAsync(Guid id)
        {
            throw new NotImplementedException();
            //if (!Cache.ContainsKey(id))
            //{
            //    throw new EntityNotFoundException(string.Format(Errors.Dishes_Dish_with_id__0__not_found, id));
            //}

            //return Task.FromResult(Cache[id]);
        }

        public Task<DishResponse> CreateDishAsync(DishRequest dish)
        {
            throw new NotImplementedException();

            //var newId = Guid.NewGuid();

            //if (Cache.Any(x => x.Value.Name == dish.Name))
            //{
            //    throw new NameAlreadyExistsException(string.Format(Errors.Dishes_Dish_with_name__0__already_exist,
            //        dish.Name));
            //}

            //Cache.Add(newId, new DishResponse()
            //{
            //    Description = dish.Description,
            //    Id = newId,
            //    Name = dish.Name,
            //    Photos = new List<Uri>()
            //});

            //return Task.FromResult(Cache[newId]);
        }

        public Task<DishResponse> UpdateDish(Guid id, DishRequest dish)
        {

            throw new NotImplementedException();

            //if (!Cache.ContainsKey(id))
            //{
            //    throw new EntityNotFoundException(string.Format(Errors.Dishes_Dish_with_id__0__not_found, id));
            //}

            //if (Cache.Any(x => x.Value.Name == dish.Name))
            //{
            //    throw new NameAlreadyExistsException(string.Format(Errors.Dishes_Dish_with_name__0__already_exist,
            //        dish.Name));
            //}

            //var item = Cache[id];

            //item.Description = dish.Description;
            //item.Name = dish.Name;
            //item.Photos = dish.Photos;
            //return Task.FromResult(item);
        }

        public Task DeleteDishAsync(Guid id)
        {
            throw new NotImplementedException();

            //Cache.Remove(id);
            //return Task.CompletedTask;
        }
    }
}