using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace ABC_Retail.Functions
{
    public static class UploadBlob
    {
        [Function("UploadBlob")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // Get the container name and blob name from query parameters
            string containerName = req.Query["containerName"];
            string blobName = req.Query["blobName"];

            // Retrieve the connection string from environment variables
            var connectionString = Environment.GetEnvironmentVariable("AzureStorage:BlobConnectionString");
            var blobServiceClient = new BlobServiceClient(connectionString);

            // Get a reference to the blob container and ensure it exists
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            // Get a reference to the blob client
            var blobClient = containerClient.GetBlobClient(blobName);

            // Upload the blob content from the request body
            using var stream = req.Body;
            await blobClient.UploadAsync(stream, true);

            // Return a success message
            return new OkObjectResult("Blob uploaded successfully");
        }
    }
}

