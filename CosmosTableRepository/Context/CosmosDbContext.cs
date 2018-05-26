using CosmosTableRepository.Entities;
using CosmosTableRepository.Exception;
using CosmosTableRepository.Interfaces;
using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;
using System;
using System.Threading.Tasks;

namespace CosmosTableRepository.Context
{
    public class CosmosDbContext : ICosmosDbContext
    {
        /// <summary>
        /// The cloud table client
        /// </summary>
        private CloudTableClient cloudTableClient;

        /// <summary>
        /// The cloud table
        /// </summary>
        private CloudTable cloudTable;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized { get; set; }

        public CloudTableClient GetCloudTableClient()
        {
            if ((this.cloudTableClient == null))
            {
                throw new ClientNotInitializedException("Table storage client is not initialized.");
            }

            return this.cloudTableClient;
        }

        public CloudTable GetTable()
        {
            if (this.cloudTable == null)
            {
                throw new ClientNotInitializedException("Table client is not initialized.");
            }

            return this.cloudTable;
        }

        public Task InitializeAsync(CosmosDbConfig cosmosDbConfig, bool disasterRecoverySupported)
        {
            if (string.IsNullOrWhiteSpace(cosmosDbConfig.ConnectionString))
            {
                throw new ArgumentException(nameof(cosmosDbConfig.ConnectionString));
            }

            if (string.IsNullOrWhiteSpace(cosmosDbConfig.TableName))
            {
                throw new ArgumentException(nameof(cosmosDbConfig.TableName));
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(cosmosDbConfig.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            var tableConnectionPolicy = new TableConnectionPolicy();
            tableConnectionPolicy.MaxRetryWaitTimeInSeconds = cosmosDbConfig.RetryIntervalInSeconds;
            tableConnectionPolicy.MaxConnectionLimit = cosmosDbConfig.MaxConnectionLimit;
            tableConnectionPolicy.EnableEndpointDiscovery = disasterRecoverySupported;
            tableConnectionPolicy.MaxRetryAttemptsOnThrottledRequests = cosmosDbConfig.RetryCountOnThrottling;
            tableConnectionPolicy.UseTcpProtocol = true;

            // Create a table client for interacting with the table service
            this.cloudTableClient = storageAccount.CreateCloudTableClient(tableConnectionPolicy,
                Microsoft.Azure.CosmosDB.ConsistencyLevel.Session);
            this.IsInitialized = true;
            cloudTable = cloudTableClient.GetTableReference(cosmosDbConfig.TableName);
            var cloudtable = (CloudTable)cloudTable;
            return cloudtable.CreateIfNotExistsAsync();
        }
    }
}
