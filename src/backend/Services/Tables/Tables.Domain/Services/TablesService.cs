using Domain.Core.Exceptions;
using Domain.Core.Models;
using Domain.Core.Repositories;
using Microsoft.Extensions.Logging;
using Tables.Domain.Models;
using Tables.Domain.Repository.Specifications;

namespace Tables.Domain.Services
{
    public class TablesService : ITablesService
    {
        private readonly ILogger<TablesService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRestaurantsService _restaurantsService;
        private readonly IRepository<Table> _tableRepository;

        public TablesService(ILogger<TablesService> logger, IUnitOfWork unitOfWork,
            IRestaurantsService restaurantsService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _restaurantsService = restaurantsService;
            _tableRepository = _unitOfWork.Repository<Table>();
        }

        public async Task<PagedList<Table>> GetTablesAsync(Guid restaurantId, int pageNumber, int pageSize)
        {
            var pagedSpecification = new PagedRestaurantTablesSpecification(restaurantId, pageNumber, pageSize);
            var table = await _tableRepository.FindAsync(pagedSpecification);
            var totalCount = await _tableRepository.CountAsync(x => x.RestaurantId == restaurantId);
            return new PagedList<Table>(table, totalCount, pageNumber, pageSize);
        }

        public async Task<Table> GetTableByIdAsync(Guid id)
        {
            var table = await _tableRepository.FindByIdAsync(id);

            if (table == null)
            {
                throw new EntityNotFoundException(nameof(Table));
            }

            return table;
        }

        public async Task<Table> CreateTableAsync(Table table)
        {
            if (string.IsNullOrEmpty(table.Name))
            {
                throw new ArgumentException($"{nameof(table.Name)} is null or empty");
            }

            if (table.NumberOfPlaces <= 0)
            {
                throw new ArgumentException($"{nameof(table.NumberOfPlaces)} must be greater than 0");
            }

            try
            {
                await _restaurantsService.CheckRestaurantExistAsync(table.RestaurantId);
            }
            catch (EntityNotFoundException)
            {
                throw new ArgumentException($"{nameof(table.RestaurantId)} not exist");
            }

            var duplicationTables =
                await _tableRepository.FindAsync(
                    new TableNameInRestaurantSpecification(table.Name, table.RestaurantId));

            if (duplicationTables.Any())
            {
                _logger.LogInformation($"Same table with name already exist in restaurant tables");
                throw new EntityAlreadyExistsException();
            }

            await _tableRepository.AddAsync(table);
            _unitOfWork.Complete();

            return table;
        }

        public async Task<Table> UpdateTable(Table table)
        {
            if (string.IsNullOrEmpty(table.Name))
            {
                throw new ArgumentException($"{nameof(table.Name)} is null or empty");
            }

            if (table.NumberOfPlaces <= 0)
            {
                throw new ArgumentException($"{nameof(table.NumberOfPlaces)} must be greater than 0");
            }

            var tableWithSameId = await _tableRepository.FindByIdAsync(table.Id);

            if (tableWithSameId == null)
            {
                throw new EntityNotFoundException(nameof(Table));
            }

            if (tableWithSameId.RestaurantId != table.RestaurantId)
            {
                _logger.LogError(
                    $"User change restaurant id from {tableWithSameId.RestaurantId} to {table.RestaurantId}");
                throw new ArgumentException($"{nameof(table.RestaurantId)} cannot be changed");
            }

            if (tableWithSameId.Name != table.Name)
            {
                _logger.LogInformation($"User change table name from {tableWithSameId.Name} to {table.Name}");

                var duplicationTables =
                    await _tableRepository.FindAsync(
                        new TableNameInRestaurantSpecification(table.Name, tableWithSameId.RestaurantId));

                if (duplicationTables.Any())
                {
                    _logger.LogInformation($"Same table with name already exist in restaurant tables");
                    throw new EntityAlreadyExistsException();
                }
            }

            tableWithSameId.Name = table.Name;
            tableWithSameId.NumberOfPlaces = table.NumberOfPlaces;

            _tableRepository.Update(tableWithSameId);
            _unitOfWork.Complete();

            return tableWithSameId;
        }

        public async Task DeleteTableAsync(Guid id)
        {
            var table = await _tableRepository.FindByIdAsync(id);

            if (table == null)
            {
                throw new EntityNotFoundException(nameof(Table));
            }

            _tableRepository.Remove(table);
            _unitOfWork.Complete();
        }
    }
}