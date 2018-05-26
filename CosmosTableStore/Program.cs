using CosmosTableStore.Entities;
using Microsoft.Azure.CosmosDB.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosTableStore
{
    class Program
    {
        static void Main(string[] args)
        {
            TestTableStorageDbAsync().GetAwaiter().GetResult();
            Console.ReadKey();
        }

        private static async Task TestTableStorageDbAsync()
        {
            var TableDbConfig = new CosmosTableRepository.Entities.CosmosDbConfig
            {
                ConnectionString =
                    "{connection string to table storage in cosmos db}",
                TableName = "{Desired table name in cosmos db}"
            };

            Console.WriteLine("Creating cosmos db context.");
            var documentDbTableContext = new CosmosTableRepository.Context.CosmosDbContext();
            await documentDbTableContext.InitializeAsync(TableDbConfig, true).ConfigureAwait(false);
            Console.WriteLine("Cosmos db context created succesfully.");


            var repository = new CosmosTableRepository.Repository.TableStorageRepository<CustomerEntity>(documentDbTableContext);
            var testEntity = new CustomerEntity("bedi", "aman")
            {
                Email = "amanbedi@test.com",
                PhoneNumber = "1234"
            };

            Console.WriteLine("Inserting entity to table.");
            Console.WriteLine("Entity Details :");
            DisplayEntities(testEntity);
            await repository.InsertTableEntityAsync(testEntity).ConfigureAwait(false);
            Console.WriteLine("Inserted successfully.");

            List<CustomerEntity> customerList = new List<CustomerEntity>();
            for (int i = 0; i < 10; i++)
            {
                customerList.Add(new CustomerEntity("Smith", string.Format("{0}", i.ToString("D4")))
                {
                    Email = string.Format("{0}@contoso.com", i.ToString("D4")),
                    PhoneNumber = string.Format("425-555-{0}", i.ToString("D4"))
                });
            }

            Console.WriteLine("Inserting a batch of entities to table.");
            Console.WriteLine("Batch Entity Details :");
            DisplayEntities(customerList);
            await repository.InsertTableEntitiesBatchAsync(customerList).ConfigureAwait(false);
            Console.WriteLine("Inserted successfully.");

            Console.WriteLine("Retrieving entities from table by partition key.");
            var getByPartition = repository.GetTableEntitiesByPartitionKeyAsync("Smith").ConfigureAwait(false).GetAwaiter().GetResult().ToList();
            Console.WriteLine("Successfully retrieved entities from table by partition key.");
            Console.WriteLine("Retrieved Range of Entity Details :");
            DisplayEntities(getByPartition);

            Console.WriteLine("Retrieving range of entities from table by partition key & row key.");
            var getRangeInPartition = repository.GetTableEntitiesRangeInPartitionAsync("Smith", "0008").ConfigureAwait(false).GetAwaiter().GetResult().ToList();
            Console.WriteLine("Successfully retrieved range of entities from table by partition key & row key.");
            Console.WriteLine("Retrieved Range of Entity Details :");
            DisplayEntities(getRangeInPartition);

            Console.WriteLine("Retrieving single entity from table by partition key & row key.");
            var getEntity = repository.GetTableEntityAsync("Smith", "0008").ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine("Successfully retrieved single entity from table by partition key & row key.");
            Console.WriteLine("Retrieved Entity Details :");
            DisplayEntities(getEntity);

            Console.WriteLine("Retrieving range of entities from table by row key.");
            var getEntitiesByRowKey = repository.GetTableEntityAsyncByRowKey("0008").ConfigureAwait(false).GetAwaiter().GetResult().ToList();
            Console.WriteLine("Successfully retrieved range of entities from table by row key.");
            Console.WriteLine("Retrieved Range of Entity Details :");
            DisplayEntities(getEntitiesByRowKey);

            Console.WriteLine("Updating entity from table by partition key & row key.");
            Console.WriteLine("Entity details :");
            var testEntityUpdated = new CustomerEntity("bedi", "aman")
            {
                Email = "amanbedi@contoso.com",
                PhoneNumber = "5678"
            };
            DisplayEntities(testEntityUpdated);
            var update = repository.RetrieveAndReplaceTableEntityAsync("bedi", "aman", testEntityUpdated);
            Console.WriteLine("Succesfully updated entity from table by partition key & row key.");

            Console.WriteLine("Upserting entity from table by partition key & row key.");
            Console.WriteLine("Entity details :");
            var UpdatedEntity = new CustomerEntity("Smith", "0001")
            {
                Email = "amanbedi@contosotest.com",
                PhoneNumber = "567899"
            };
            DisplayEntities(UpdatedEntity);
            var updated = repository.InsertOrReplaceTableEntityAsync(UpdatedEntity).GetAwaiter().GetResult();
            Console.WriteLine("Succesfully upserted entity from table by partition key & row key.");

            Console.WriteLine("Retrieving all entities from table.");
            var allEntities = repository.GetAllTableEntitiesAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine("Succesfully retrieved all entities from table.");
            Console.WriteLine("Entity details :");
            DisplayEntities(allEntities.ToList());

            Console.WriteLine("Retrieving all entities from table by query.");
            var queryEntites = repository.GetTableEntitiesByQueryAsync(new TableQuery<CustomerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"))).GetAwaiter().GetResult().ToList();
            Console.WriteLine("Succesfully retrieved all entities from table by query.");
            Console.WriteLine("Entity details :");
            DisplayEntities(queryEntites.ToList());

            Console.WriteLine("Deleting entity from table by partition key & row key.");
            var del = repository.DeleteTableEntityAsync("aman", "bedi").ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine("Succesfully deleted entity from table by partition key & row key.");

            Console.WriteLine("Deleting table.");
            var deleteTable = repository.DeleteTableAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine("Succesfully deleted table.");
        }

        private static void DisplayEntities(List<CustomerEntity> customerEntities)
        {
            int i = 1;
            foreach (var item in customerEntities)
            {
                Console.WriteLine($"Entity : {i}");
                Console.WriteLine($"Partition Key / Last Name : {item.PartitionKey}");
                Console.WriteLine($"Row Key / First Name: {item.RowKey}");
                Console.WriteLine($"Email : {item.Email}");
                Console.WriteLine($"Phone : {item.PhoneNumber}");
                Console.WriteLine("------------------------------------------------------------------------------------------------");
                i++;
            }
        }

        private static void DisplayEntities(CustomerEntity customerEntity)
        {
            Console.WriteLine($"Partition Key / Last Name : {customerEntity.PartitionKey}");
            Console.WriteLine($"Row Key / First Name: {customerEntity.RowKey}");
            Console.WriteLine($"Email : {customerEntity.Email}");
            Console.WriteLine($"Phone : {customerEntity.PhoneNumber}");
            Console.WriteLine("------------------------------------------------------------------------------------------------");
        
        }
    }
}
