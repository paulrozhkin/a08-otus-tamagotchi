using System;
using System.Threading.Tasks;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public interface ITablesService
    {
        public Task<PaginationResponse<TableResponse>> GetTablesAsync(Guid restaurantId, int pageNumber, int pageSize);

        public Task<TableResponse> GetTableByIdAsync(Guid tableId);

        public Task<TableResponse> CreateTableAsync(Guid restaurantId, TableRequest table);

        public Task<TableResponse> UpdateTableAsync(Guid restaurantId, Guid tableId, TableRequest table);

        public Task DeleteTableAsync(Guid tableId);
    }
}