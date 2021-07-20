using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WisdomPetMedicine.Hospital.Infrastructure
{
    public static class HealthChecksBuilderExtensions
    {
        public static IHealthChecksBuilder AddCosmosDbCheck(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            return builder.Add(new HealthCheckRegistration("WisdomPetMedicine", new WisdomPetMedicineCosmosDbHealthCheck(configuration), HealthStatus.Unhealthy, null));
        }
    }
}