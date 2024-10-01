using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ABC_Retail.Functions
{
    public class AzureFunction
    {
        private readonly ILogger<AzureFunction> _logger;

        // Constructor for dependency injection of the logger
        public AzureFunction(ILogger<AzureFunction> logger)
        {
            _logger = logger;
        }

        [Function("HTTPTest")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request."); // Log the request
            return new OkObjectResult("Welcome to Azure Functions!"); // Return a welcome message
        }
    }
}
