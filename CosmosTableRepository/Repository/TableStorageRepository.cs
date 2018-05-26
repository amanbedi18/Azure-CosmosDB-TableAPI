using Microsoft.Azure.CosmosDB.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using CosmosTableRepository.Interfaces;

namespace CosmosTableRepository.Repository
{
    /// <summary>
    /// Table Storage Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="CosmosRepoPoC.Contracts.ITableStorageRepository{T}" />
    public class TableStorageRepository<T> : ITableStorageRepository<T> where T : TableEntity, new()
    {

        /// <summary>
        /// The cloud table
        /// </summary>
        private readonly CloudTable cloudTable;

        /// <summary>
        /// The table client
        /// </summary>
        private readonly CloudTableClient tableClient;

        public TableStorageRepository(ICosmosDbContext documentDbContext)
        {
            // Create the table client.
            this.tableClient = documentDbContext.GetCloudTableClient();

            // Retrieve a reference to the table.
            this.cloudTable = documentDbContext.GetTable();

        }

        public async Task<bool> DeleteTableAsync()
        {
            return await cloudTable.DeleteIfExistsAsync();
        }

        public async Task<IEnumerable<T>> GetAllTableEntitiesAsync()
        {
            TableContinuationToken token = null;
            var entities = new List<T>();
            do
            {
                var queryResult = await cloudTable.ExecuteQuerySegmentedAsync(new TableQuery<T>(), token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);
            return entities;
        }

        public async Task<int> DeleteTableEntityAsync(string rowKey, string partitionKey)
        {
            // Create a retrieve operation that expects a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            // Execute the operation.
            TableResult retrievedResult = await cloudTable.ExecuteAsync(retrieveOperation);

            // Assign the result to a CustomerEntity.
            var deleteEntity = (T)retrievedResult.Result;

            // Create the Delete TableOperation.
            TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
            // Execute the operation.
            var result = cloudTable.Execute(deleteOperation);
            return result.HttpStatusCode;
        }

        public async Task<IEnumerable<T>> GetTableEntitiesByPartitionKeyAsync(string partitionKey)
        {
            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            // Print the fields for each customer.
            var result = cloudTable.ExecuteQuery(query);
            return result;
        }

        public async Task<IEnumerable<T>> GetTableEntitiesRangeInPartitionAsync(string partitionKey, string rowKey)
        {
            // Create the table query.
            TableQuery<T> rangeQuery = new TableQuery<T>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, rowKey)));

            // Loop through the results, displaying information about the entity.
            var result = cloudTable.ExecuteQuery(rangeQuery);
            return result;
        }

        public async Task<IEnumerable<T>> GetTableEntitiesByQueryAsync(TableQuery<T> tableQuery)
        {
            var entities = new List<T>();

            TableContinuationToken token = null;
            do
            {
                var queryResult = await cloudTable.ExecuteQuerySegmentedAsync(tableQuery, token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return entities;
        }

        public async Task<T> GetTableEntityAsync(string partitionKey, string rowKey)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = await cloudTable.ExecuteAsync(retrieveOperation);

            var entity = (T)retrievedResult.Result;
            return entity;
        }

        public async Task<IEnumerable<T>> GetTableEntityAsyncByRowKey(string rowKey)
        {
            // Create the table query.
            TableQuery<T> rangeQuery = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));

            // Loop through the results, displaying information about the entity.
            var result = cloudTable.ExecuteQuery(rangeQuery);
            return result;
        }

        public async Task<int> InsertOrReplaceTableEntityAsync(T updatedEntity)
        {
            // Create the InsertOrReplace TableOperation.
            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(updatedEntity);

            // Execute the operation. Because a 'Fred Jones' entity already exists in the
            // 'people' table, its property values will be overwritten by those in this
            // CustomerEntity. If 'Fred Jones' didn't already exist, the entity would be
            // added to the table.
            var result = await cloudTable.ExecuteAsync(insertOrReplaceOperation);
            return result.HttpStatusCode;
        }

        public async Task InsertTableEntitiesBatchAsync(System.Collections.Generic.List<T> tableEntities)
        {
            // Create the batch operation.
            TableBatchOperation batchOperation = new TableBatchOperation();

            tableEntities.ForEach(t => batchOperation.Insert(t));
            await cloudTable.ExecuteBatchAsync(batchOperation);
        }

        public async Task<int> InsertTableEntityAsync(T tableEntity)
        {
            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(tableEntity);

            // Execute the insert operation.
            var result = await cloudTable.ExecuteAsync(insertOperation);
            return result.HttpStatusCode;
        }

        public async Task RetrieveAndReplaceTableEntityAsync(string partitionKey, string rowKey, T updatedEntity)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            // Execute the operation.
            TableResult retrievedResult = await cloudTable.ExecuteAsync(retrieveOperation);

            // Assign the result to a CustomerEntity object.
            var updateEntity = (T)retrievedResult.Result;

            // Create the Replace TableOperation.
            TableOperation updateOperation = TableOperation.Replace(updatedEntity);

            // Execute the operation.
            await cloudTable.ExecuteAsync(updateOperation);
        }
    }
}
