using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Dapper;

using MerchApi.Domain.AggregationModels.MerchAggregate;
using MerchApi.Infrastructure.Repositories.Infrastructure.Interfaces;

using Npgsql;

namespace MerchApi.Infrastructure.Repositories.Implementation
{
    public class GiveOutMerchRequestRepository : IGiveOutMerchRequestRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private const int Timeout = 5;

        public GiveOutMerchRequestRepository(
            IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory,
            IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }

        public async Task<GiveOutMerchRequest> CreateAsync(GiveOutMerchRequest itemToCreate, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO skus (id, name, item_type_id, clothing_size)
                VALUES (@SkuId, @Name, @ItemTypeId, @ClothingSize);
                INSERT INTO stocks (sku_id, quantity, minimal_quantity)
                VALUES (@SkuId, @Quantity, @MinimalQuantity);";

            var parameters = new
            {
                SkuId = itemToCreate.EmployeeId
            };

            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(itemToCreate);

            return itemToCreate;
        }

        public async Task<IReadOnlyList<GiveOutMerchRequest>> FindByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT *
                FROM merch_requests               
                WHERE employeeId = ANY(@employeeId);";

            var parameters = new
            {
                employeeId
            };

            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            var merchRequests = await connection.QueryAsync<Models.MerchRequest, Models.MerchType, GiveOutMerchRequest>(commandDefinition,
                (merchRequest, merchType) => new GiveOutMerchRequest(merchRequest.EmployeeId, new MerchType(merchType.Id, merchType.Name)));

            var result = merchRequests.ToList();

            foreach (var stockItem in result)
            {
                _changeTracker.Track(stockItem);
            }

            return result;
        }
    }
}