using Domain.Core.Exceptions;
using Domain.Core.Models;
using Domain.Core.Repositories;
using Domain.Core.Repositories.Specifications;
using Microsoft.Extensions.Logging;
using Orders.Domain.Models;
using Orders.Domain.Repositories.Specifications;

namespace Orders.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Order> _restaurantsRepository;
        private readonly IMenuAmountService _menuAmountService;

        public OrderService(ILogger<OrderService> logger, IUnitOfWork unitOfWork,
            IMenuAmountService menuAmountService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _restaurantsRepository = _unitOfWork.Repository<Order>();
            _menuAmountService = menuAmountService;
        }

        public async Task<PagedList<Order>> GetOrdersAsync(int pageNumber, int pageSize, Guid clientId)
        {
            var paginationSpecification = new PagedOrderWithClientFilterSpecification(clientId, pageNumber, pageSize);
            var orders = (await _restaurantsRepository.FindAsync(paginationSpecification)).ToList();
            var totalCount = await _restaurantsRepository.CountAsync(x => x.ClientId == clientId);

            return new PagedList<Order>(orders, totalCount, pageNumber, pageSize);
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            var amount = await _menuAmountService.CalculateAmountForMenuPositions(order.Menu);
            order.AmountOfRubles = amount;

            await _restaurantsRepository.AddAsync(order);
            _unitOfWork.Complete();

            return order;
        }
    }
}