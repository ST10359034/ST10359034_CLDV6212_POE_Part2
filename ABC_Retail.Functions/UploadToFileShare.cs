using Azure;
using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace ABC_Retail.Functions
{
    public static class UploadToFileShare
    {
        [Function("UploadToFileShare")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // Define the share name and get the file name from the query parameters
            string shareName = "contracts";
            string fileName = req.Query["fileName"];

            // Get the connection string from environment variables
            var connectionString = Environment.GetEnvironmentVariable("AzureStorage:FileConnectionString");
            var shareClient = new ShareClient(connectionString, shareName);

            // Ensure the share exists
            await shareClient.CreateIfNotExistsAsync();

            // Get a reference to the root directory and file
            var directoryClient = shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);

            // Upload the file stream to Azure Files
            using var stream = req.Body;
            await fileClient.CreateAsync(stream.Length);
            await fileClient.UploadRangeAsync(new HttpRange(0, stream.Length), stream);

            // Return a success message
            return new OkObjectResult("File uploaded to Azure Files");
        }
    }
}

