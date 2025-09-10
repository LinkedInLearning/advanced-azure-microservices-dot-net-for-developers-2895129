using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace WisdomPetMedicine.Hospital.Infrastructure;

public class WisdomPetMedicineCosmosDbHealthCheck(IConfiguration configuration) : IHealthCheck
{
    private readonly CosmosClient cosmosClient = new CosmosClient(configuration["CosmosDb:ConnectionString"]);

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        await cosmosClient.ReadAccountAsync();
        var databaseId = configuration["CosmosDb:DatabaseId"];
        var containerId = configuration["CosmosDb:ContainerId"];
        var container = cosmosClient.GetContainer(databaseId, containerId);
        var containerProperties = await container.ReadContainerAsync(cancellationToken: cancellationToken);
        return containerProperties.StatusCode == System.Net.HttpStatusCode.OK ? HealthCheckResult.Healthy() :
            HealthCheckResult.Unhealthy();
    }
}