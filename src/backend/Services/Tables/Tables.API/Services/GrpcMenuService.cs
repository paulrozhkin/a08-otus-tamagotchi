using AutoMapper;
using Domain.Core.Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Infrastructure.Core.Localization;
using Tables.Domain.Services;
using TablesApi;

namespace Tables.API.Services
{
    public class GrpcTableService : TablesApi.Tables.TablesBase
    {
        private readonly ILogger<GrpcTableService> _logger;
        private readonly ITablesService _tablesService;
        private readonly IMapper _mapper;

        public GrpcTableService(ILogger<GrpcTableService> logger,
            ITablesService tablesService,
            IMapper mapper)
        {
            _logger = logger;
            _tablesService = tablesService;
            _mapper = mapper;
        }

        public override async Task<GetTablesResponse> GetTables(GetTablesRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.RestaurantId, out var restaurantId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument,
                    $"Argument null or empty {nameof(request.RestaurantId)}"));
            }

            var table = await _tablesService.GetTablesAsync(restaurantId, request.PageNumber, request.PageSize);

            var tablesResponse = new GetTablesResponse()
            {
                CurrentPage = table.CurrentPage,
                PageSize = table.PageSize,
                TotalCount = table.TotalCount
            };

            var tableDto = _mapper.Map<List<Table>>(table);

            tablesResponse.Tables.AddRange(tableDto);

            return tablesResponse;
        }

        public override async Task<GetTableResponse> GetTable(GetTableRequest request,
            ServerCallContext context)
        {
            try
            {
                var table = await _tablesService.GetTableByIdAsync(Guid.Parse(request.Id));
                var tableDto = _mapper.Map<Table>(table);

                var tableResponse = new GetTableResponse()
                {
                    Table = tableDto
                };

                return tableResponse;
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"{Errors.Entities_Entity_not_found}, Table {request.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }

        public override async Task<CreateTableResponse> CreateTable(CreateTableRequest request,
            ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.Table.RestaurantId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument,
                    $"Argument null or empty {nameof(request.Table.RestaurantId)}"));
            }

            var tableModel = _mapper.Map<Domain.Models.Table>(request);

            try
            {
                var createdTable = await _tablesService.CreateTableAsync(tableModel);

                return new CreateTableResponse()
                {
                    Table = _mapper.Map<Table>(createdTable)
                };
            }
            catch (ArgumentException e)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, e.Message));
            }
            catch (EntityAlreadyExistsException)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, Errors.Entities_Entity_already_exits));
            }
        }

        public override async Task<UpdateTableResponse> UpdateTable(UpdateTableRequest request,
            ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.Table.RestaurantId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument,
                    $"Argument null or empty {nameof(request.Table.RestaurantId)}"));
            }

            var tableModel = _mapper.Map<Domain.Models.Table>(request);

            try
            {
                var updateTable = await _tablesService.UpdateTable(tableModel);

                return new UpdateTableResponse()
                {
                    Table = _mapper.Map<Table>(updateTable)
                };
            }
            catch (ArgumentException e)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, e.Message));
            }
            catch (EntityAlreadyExistsException)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, Errors.Entities_Entity_already_exits));
            }
            catch (EntityNotFoundException)
            {
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }

        public override async Task<Empty> DeleteTable(DeleteTableRequest request, ServerCallContext context)
        {
            var idForDelete = request.Id;

            try
            {
                await _tablesService.DeleteTableAsync(Guid.Parse(idForDelete));
                return new Empty();
            }
            catch (EntityNotFoundException)
            { 
                throw new RpcException(new Status(StatusCode.NotFound, Errors.Entities_Entity_not_found));
            }
        }
    }
}