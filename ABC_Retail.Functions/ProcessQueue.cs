using Azure.Storage.Queues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ABC_Retail.Functions
{
    public static class ProcessQueue
    {
        [Function("ProcessQueue")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string queueName = "transactions-queue";
            var connectionString = Environment.GetEnvironmentVariable("AzureStorage:QueueConnectionString");

            var queueClient = new QueueClient(connectionString, queueName);
            await queueClient.CreateIfNotExistsAsync(); // Create queue if it doesn't exist

            string message = req.Query["message"];
            await queueClient.SendMessageAsync(message); // Send message to the queue

            var queueMessage = await queueClient.ReceiveMessageAsync(); // Receive a message
            string receivedMessage = queueMessage.Value.Body.ToString();

            return new OkObjectResult($"Message sent: {message}, Message received: {receivedMessage}");
        }
    }
}


