# Cosmos DB Table API Repository

## Introduction
Cosmos DB Table API Repository is a library which provides data store strategy for accessing cosmos db via the Table API. It provides seamless & managed way to perform CRUD operations on cosmos DB tables and query the same. The library also implements best practices to work with Table API.

The current implementation targets all projects to .NET Framework 4.5.2 as Microsoft.Azure.CosmosDB.Table is not yet supported for .NET Standard. Please track https://github.com/Azure/azure-documentdb-dotnet/issues/344 for the same. The same code base can be re-targeted to .NET core once the support is enabled.

## Architecture
Cosmos DB Table API Repository has two components.
* Common Library for Cosmos DB Table API.
* Test Project demonstrating the capabilities of the repository.

## Common Library
Cosmos DB Table API Repository has common library that communicates with Cosmos DB. It provides functionality to
* Define entities for Tables
* Supports setting TTL for operations
* Querying Tables
* Creating/Insert/Upsert/Delete entities to tables
* Create Table / Delete Table

The components of library are as shown below:

### Cosmos DB Context
The recommendation is to use one **_CloudTableClient_** per App Domain with appropriate settings of preferred locations and consistency Level. To support this, library provides **_CosmosDbContext_**. It is recommended to register the context as singleton in appropriate dependency containers. Below are the methods in Cosmos DB Context.

| Method                                                                                       | Description   |
| -------------------------------------------------------------------------------------------- |---------------|
| Task InitializeAsync(CosmosDbConfig cosmosDbConfig, bool disasterRecoverySupported)      | Initializes **_CloudTableClient_** using config. Disaster Recovery Supported is used to indicate whether multiple regions are configured for Cosmos db account. |
| CloudTableClient GetCloudTableClient()                                                       | Gets the Cosmos DB Cloud Table client. Throws error if initialization is not done.|
| CloudTable GetTable()                                                                        | Gets the Cloud Table. Throws error if initialization is not done.|

During **_InitializeAsync_**, below are the configurations used by default
1. Direct TCP Connection Protocol
2. Session as default Consistency Level in client
3. Retry Options as specified in **_CosmosDbConfig_**
4. Opens the client to avoid delay in first operation

###  Cosmos Db Config
Specifies the configuration to initialize **_CloudTableClient_**. Following are properties.

| Property                                                                                       | Description   |
| --------------------------------------------------------------------------------------------   |---------------|
| MediaRequestTimeOutInSeconds                                                                   | Specifies Media Request Time out for cosmos Db **_CloudTableclient_** |
| RequestTimeoutInSeconds                                                                        | Specifies request timeout |
| MaxConnectionLimit                                                                             | Specifies maximum number of concurrent connections allowed for the target service endpoint in the Azure Cosmos Db database service. |
| ConnectionString                                                                               | Specifies Connection String |
| RetryCountOnThrottling                                                                         | Specifies Retry Count if a request is throttled |
| RetryIntervalInSeconds                                                                         | Specifies Retry Interval if a request is throttled |
| TableName                                                                                      | Specifies the table name|

## Data Store Strategy
Any C# Class can be treated as a table entity as long as it derives from **_Microsoft.Azure.CosmosDB.Table.TableEntity_** & initializes the **_PartitionKey_** & **_RowKey_** properties of the base class.The Repository. Query to Cosmos db depends on type of request. Below are the request options provided.

| Method                                                                                       | Description   |
| -------------------------------------------------------------------------------------------- |---------------|
| Task<int> InsertTableEntityAsync(T tableEntity)                                              | Inserts the Table Entity. |
| Task InsertTableEntitiesBatchAsync(List<T> tableEntities)                                    | Inserts a batch of Table Entities.|
| Task<IEnumerable<T>> GetTableEntitiesByPartitionKeyAsync(string partitionKey)                | Gets Range of Table Entities by Partition Key.|
| Task<IEnumerable<T>> GetTableEntitiesRangeInPartitionAsync(string partitionKey, string rowKey)| Gets Range of Table Entities by Partition Key & Row Key.|
| Task<T> GetTableEntityAsync(string partitionKey, string rowKey)                              | Gets the Table Entity by Row Key & Partition Key.|
| Task<IEnumerable<T>> GetTableEntityAsyncByRowKey(string rowKey)                              | Gets Range of Table Entities by Row Key.|
| Task RetrieveAndReplaceTableEntityAsync(string partitionKey, string rowKey, T updatedEntity) | Retrieves a Table Entity via the provided Partition Key & Row Key & updates the same with the updated Table Entity.|
| Task<int> InsertOrReplaceTableEntityAsync(T updatedEntity)                                   | Inserts or Replaces an Entity / existing Entity.|
| Task<IEnumerable<T>> GetTableEntitiesByQueryAsync(TableQuery<T> tableQuery)                  | Gets Range of Table Entities via the provided TableQuery.|
| Task<IEnumerable<T>> GetAllTableEntitiesAsync()                                              | Gets all Table Entities.|
| Task<int> DeleteTableEntityAsync(string rowKey, string partitionKey)                         | Deletes Table Entity with provided Row Key & Partition Key.|
| Task<bool> DeleteTableAsync()                                                                | Deletes the Table.| 

## Consistency Level
Library Provides Specifying Consistency Level at
* Client Level: Using **_CosmosDbConfig_**
* Request Level: With help of ConsistencyLevel property in **_CosmosDbOptions_**

## Test Project
The Project CosmosTableStore is a console application that has reference implementation of using library. The project
Provides approach of defining custom table entity. **_CustomerEntity_** is defined that inherits from **_Microsoft.Azure.CosmosDB.Table.TableEntity_**
* Creates **_CosmosDbContext_** using provided **_CosmosDbConfig_** (ensure to replace the **_ConnectionString_** & **_TableName_** properties to actual values in object initializer)
* Invokes Insert/Insert in batch/Update/Upsert/Retrieve entities by various options/Delete entity Operations from cosmos Db Tables by initializing **_TableStorageRepository_** by the **_CosmosDbContext_** & calling the implemented methods.
* Implementation to retrieve range of Table Entities using **_TableQuery_** & **_ContinuationToken_**
