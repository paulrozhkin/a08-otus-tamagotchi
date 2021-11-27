using System;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Exceptions;
using Grpc.Core;
using Infrastructure.Core.Localization;
using TablesApi;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Services
{
    public class TablesService : ITablesService
    {
        private readonly Tables.TablesClient _tablesClient;
        private readonly IMapper _mapper;

        public TablesService(Tables.TablesClient tablesClient, IMapper mapper)
        {
            _tablesClient = tablesClient;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<TableResponse>> GetTablesAsync(Guid restaurantId, int pageNumber,
            int pageSize)
        {
            var tableResponse = await _tablesClient.GetTablesAsync(new GetTablesRequest
                {PageNumber = pageNumber, PageSize = pageSize, RestaurantId = restaurantId.ToString()});

            return _mapper.Map<PaginationResponse<TableResponse>>(tableResponse);
        }

        public async Task<TableResponse> GetTableByIdAsync(Guid tableId)
        {
            try
            {
                var tableResponse =
                    await _tablesClient.GetTableAsync(new GetTableRequest {Id = tableId.ToString()});
                return _mapper.Map<TableResponse>(tableResponse.Table);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found,
                    tableId));
            }
        }

        public async Task<TableResponse> CreateTableAsync(Guid restaurantId, TableRequest table)
        {
            try
            {
                var tableDto = _mapper.Map<Table>(table);
                tableDto.RestaurantId = restaurantId.ToString();

                var dishResponse = await _tablesClient.CreateTableAsync(new CreateTableRequest
                {
                    Table = tableDto
                });

                return _mapper.Map<TableResponse>(dishResponse.Table);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.InvalidArgument)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                throw new EntityAlreadyExistsException(Errors.Entities_Entity_already_exits);
            }
        }

        public async Task<TableResponse> UpdateTableAsync(Guid restaurantId, Guid tableId,
            TableRequest table)
        {
            try
            {
                var tableForRequest = _mapper.Map<Table>(table);
                tableForRequest.Id = tableId.ToString();
                tableForRequest.RestaurantId = restaurantId.ToString();

                var tableResponse = await _tablesClient.UpdateTableAsync(new UpdateTableRequest
                {
                    Table = tableForRequest
                });

                return _mapper.Map<TableResponse>(tableResponse.Table);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.InvalidArgument)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found,
                    tableId));
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                throw new EntityAlreadyExistsException(Errors.Entities_Entity_already_exits);
            }
        }

        public async Task DeleteTableAsync(Guid tableId)
        {
            try
            {
                await _tablesClient.DeleteTableAsync(new DeleteTableRequest {Id = tableId.ToString()});
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new EntityNotFoundException(string.Format(Errors.Entities_Entity_with_id__0__not_found,
                    tableId));
            }
        }
    }
}