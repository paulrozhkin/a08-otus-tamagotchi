using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Models.Dish;
using Restaurant.Core.Abstractions.Repositories;
using Restaurant.Core.Domain;
using System.Threading.Tasks;

namespace Restaurant.API.Controllers
{
    /// <summary>
    /// Блюда
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DishesController
        : ControllerBase
    {
        private readonly IRepository<KitchenOrderDish> _dishRepository;
        private readonly IMapper _mapper;

        public DishesController(
            IRepository<KitchenOrderDish> dishRepository,
            IMapper mapper)
        {
            _dishRepository = dishRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Обновить данные блюда в заказе
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> UpdateAsync(DishUpdateRequest request)
        {
            var dish = await _dishRepository.GetByIdAsync(request.Id);
            if (dish == null)
            {
                return NotFound();
            }
            _mapper.Map(request, dish);
            await _dishRepository.UpdateAsync(dish);
            return Ok();
        }
    }
}
