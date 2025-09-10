using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WisdomPetMedicine.Hospital.Domain.Repositories;
using WisdomPetMedicine.Hospital.Domain.ValueObjects;
using WisdomPetMedicine.Hospital.Infrastructure;

namespace WisdomPetMedicine.Hospital.Projector;

public class PatientsProjector(IConfiguration configuration,
                               IPatientAggregateStore patientAggregateStore)
{
    [Function(nameof(PatientsProjector))]
    public async Task Run([CosmosDBTrigger(
        databaseName: "WisdomPetMedicine",
        containerName: "Patients",
        Connection = "CosmosDbConnectionString",
        CreateLeaseContainerIfNotExists = true,
        LeaseContainerName = "leases")] IReadOnlyList<CosmosEventData> input, FunctionContext context)
    {
        var logger = context.GetLogger("PatientsProjector");
        if (input == null || !input.Any())
        {
            return;
        }

        logger.LogInformation("Items received: " + input.Count);

        using var conn = new SqlConnection(configuration.GetConnectionString("Hospital"));
        conn.EnsurePatientsTable();

        foreach (var item in input)
        {
            var patientId = Guid.Parse(item.AggregateId.Replace("Patient-", string.Empty));
            var patient = await patientAggregateStore.LoadAsync(PatientId.Create(patientId));

            conn.InsertPatient(patient);
            logger.LogInformation(item.Data);
        }

        conn.Close();
    }
}