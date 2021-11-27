using Domain.Core.Models;
using Tables.Domain.Models;

namespace Tables.Domain.Services
{
    public interface ITablesService
    {
        public Task<PagedList<Table>> GetTablesAsync(Guid restaurantId, int pageNumber, int pageSize);

        public Task<Table> GetTableByIdAsync(Guid id);

        public Task<Table> CreateTableAsync(Table table);

        public Task<Table> UpdateTable(Table table);

        public Task DeleteTableAsync(Guid id);
    }
}