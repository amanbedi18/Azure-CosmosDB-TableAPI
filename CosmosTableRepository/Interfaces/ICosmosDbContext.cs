using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosTableRepository.Interfaces
{
    using System.Threading.Tasks;
    using Microsoft.Azure.CosmosDB.Table;
    using Microsoft.Azure.Documents;
    using CosmosTableRepository.Entities;

    /// <summary>
    /// Represents the context for Cosmos database.
    /// The implementations of context should make sure it is registered as singleton.
    /// The default implementation of context considers direct TCP Mode and consistency level as session.
    /// </summary>
    public interface ICosmosDbContext
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        bool IsInitialized { get; set; }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <param name="cosmosDbConfig">The document database configuration.</param>
        /// <param name="disasterRecoverySupported">if set to <c>true</c> [disaster recovery supported].</param>
        /// <returns>The Task</returns>
        Task InitializeAsync(CosmosDbConfig cosmosDbConfig, bool disasterRecoverySupported);

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <returns></returns>
        CloudTable GetTable();

        /// <summary>
        /// Gets the cloud table client.
        /// </summary>
        /// <returns></returns>
        CloudTableClient GetCloudTableClient();
    }
}
