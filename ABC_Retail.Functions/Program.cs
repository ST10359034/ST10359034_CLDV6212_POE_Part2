using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.WebJobs.Extensions.Storage.Blobs;
using Microsoft.Azure.WebJobs.Extensions.Storage.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker;

var host = new HostBuilder()
    // Configure the function web application
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        // Add Application Insights telemetry for monitoring
        services.AddApplicationInsightsTelemetryWorkerService();
        // Configure Application Insights for the Functions
        services.ConfigureFunctionsApplicationInsights();
    })
    .ConfigureWebJobs(b =>
    {
        // Register Azure Storage Blob bindings for Blob Storage functions
        b.AddAzureStorageBlobs();
        // Register Azure Storage Queue bindings for Queue Storage functions
        b.AddAzureStorageQueues();
    })
    .Build(); // Build the host

host.Run(); // Run the function host


