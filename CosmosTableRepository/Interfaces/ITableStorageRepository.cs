namespace CosmosTableRepository.Interfaces
{
    using Microsoft.Azure.CosmosDB.Table;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITableStorageRepository<T> where T : TableEntity
    {
        Task<int> InsertTableEntityAsync(T tableEntity);
        Task InsertTableEntitiesBatchAsync(List<T> tableEntities);
        Task<IEnumerable<T>> GetTableEntitiesByPartitionKeyAsync(string partitionKey);
        Task<IEnumerable<T>> GetTableEntitiesRangeInPartitionAsync(string partitionKey, string rowKey);
        Task<T> GetTableEntityAsync(string partitionKey, string rowKey);
        Task<IEnumerable<T>> GetTableEntityAsyncByRowKey(string rowKey);
        Task RetrieveAndReplaceTableEntityAsync(string partitionKey, string rowKey, T updatedEntity);
        Task<int> InsertOrReplaceTableEntityAsync(T updatedEntity);
        Task<IEnumerable<T>> GetTableEntitiesByQueryAsync(TableQuery<T> tableQuery);
        Task<IEnumerable<T>> GetAllTableEntitiesAsync();
        Task<int> DeleteTableEntityAsync(string rowKey, string partitionKey);
        Task<bool> DeleteTableAsync();
    }
}
