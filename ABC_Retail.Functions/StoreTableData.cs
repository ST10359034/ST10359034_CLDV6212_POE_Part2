using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ABC_Retail.Functions
{
    public static class StoreTableData
    {
        [Function("StoreTableData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // Define the name of the Azure Table to store customer profiles
            string tableName = "CustomerProfiles";

            // Retrieve the connection string from environment variables
            var connectionString = Environment.GetEnvironmentVariable("AzureStorage:TableConnectionString");

            // Create a TableClient to interact with the Azure Table Storage
            var tableClient = new TableClient(connectionString, tableName);
            await tableClient.CreateIfNotExistsAsync(); // Ensure the table exists

            // Create a new TableEntity with partition key and unique row key
            var entity = new TableEntity("CustomerPartition", Guid.NewGuid().ToString())
            {
                { "FirstName", req.Query["FirstName"] },
                { "LastName", req.Query["LastName"] },
                { "Email", req.Query["Email"] },
                { "PhoneNumber", req.Query["PhoneNumber"] }
            };

            // Add the entity to the Azure Table
            await tableClient.AddEntityAsync(entity);

            // Return a success message
            return new OkObjectResult("Data stored in table");
        }
    }
}
