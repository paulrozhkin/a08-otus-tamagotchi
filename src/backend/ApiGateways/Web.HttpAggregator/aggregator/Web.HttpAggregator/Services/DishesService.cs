using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core.Exceptions;
using Infrastructure.Core.Localization;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public class DishesService : IDishesService
    {
        private static readonly IDictionary<Guid, DishResponse> Cache = new Dictionary<Guid, DishResponse>();

        public Task<PaginationResponse<DishResponse>> GetDishesAsync(int pageNumber, int pageSize)
        {
            return Task.FromResult(new PaginationResponse<DishResponse>()
            {
                CurrentPage = 1,
                PageSize = int.MaxValue,
                TotalCount = Cache.Count,
                Items = Cache.Values.ToList()
            });
        }

        public Task<DishResponse> GetDishByIdAsync(Guid id)
        {
            if (!Cache.ContainsKey(id))
            {
                throw new EntityNotFoundException(string.Format(Errors.Dishes_Dish_with_id__0__not_found, id));
            }

            return Task.FromResult(Cache[id]);
        }

        public Task<DishResponse> CreateDishAsync(DishRequest dish)
        {
            var newId = Guid.NewGuid();

            if (Cache.Any(x => x.Value.Name == dish.Name))
            {
                throw new NameAlreadyExistsException(string.Format(Errors.Dishes_Dish_with_name__0__already_exist,
                    dish.Name));
            }

            Cache.Add(newId, new DishResponse()
            {
                Description = dish.Description,
                Id = newId,
                Name = dish.Name,
                Photos = new List<Uri>()
            });

            return Task.FromResult(Cache[newId]);
        }

        public Task<DishResponse> UpdateDish(Guid id, DishRequest dish)
        {
            if (!Cache.ContainsKey(id))
            {
                throw new EntityNotFoundException(string.Format(Errors.Dishes_Dish_with_id__0__not_found, id));
            }

            if (Cache.Any(x => x.Value.Name == dish.Name))
            {
                throw new NameAlreadyExistsException(string.Format(Errors.Dishes_Dish_with_name__0__already_exist,
                    dish.Name));
            }

            var item = Cache[id];

            item.Description = dish.Description;
            item.Name = dish.Name;
            item.Photos = dish.Photos;
            return Task.FromResult(item);
        }

        public Task DeleteDishAsync(Guid id)
        {
            Cache.Remove(id);
            return Task.CompletedTask;
        }
    }
}