namespace CosmosTableRepository.Entities
{
    /// <summary>
    /// The cosmos db config
    /// </summary>
    public class CosmosDbConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDbConfig"/> class.
        /// </summary>
        public CosmosDbConfig()
        {
            this.MediaRequestTimeOutInSeconds = 300;
            this.RequestTimeoutInSeconds = 60;
            this.MaxConnectionLimit = 50;
        }

        /// <summary>
        /// Gets or sets the media request time out.
        /// </summary>
        /// <value>
        /// The media request time out.
        /// </value>
        public int MediaRequestTimeOutInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the request timeout in seconds.
        /// </summary>
        /// <value>
        /// The request timeout in seconds.
        /// </value>
        public int RequestTimeoutInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of concurrent connections allowed for the target
        /// service endpoint in the Azure DocumentDB database service.
        /// </summary>
        /// <value>
        /// The maximum connection limit.
        /// </value>
        public int MaxConnectionLimit { get; set; }

        /// <summary>
        /// Gets or sets the retry count on throttling.
        /// </summary>
        /// <value>
        /// The retry count on throttling.
        /// </value>
        public int RetryCountOnThrottling { get; set; }

        /// <summary>
        /// Gets or sets the retry interval in seconds.
        /// </summary>
        /// <value>
        /// The retry interval in seconds.
        /// </value>
        public int RetryIntervalInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }
    }
}
