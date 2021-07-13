using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WisdomPetMedicine.Common;
using WisdomPetMedicine.Hospital.Domain.Entities;
using WisdomPetMedicine.Hospital.Domain.Repositories;
using WisdomPetMedicine.Hospital.Domain.ValueObjects;

namespace WisdomPetMedicine.Hospital.Infrastructure
{
    public class PatientAggregateStore : IPatientAggregateStore
    {
        private readonly CosmosClient cosmosClient;
        private readonly Container patientContainer;
        public PatientAggregateStore(IConfiguration configuration)
        {
            var connectionString = configuration["CosmosDb:ConnectionString"];
            var databaseId = configuration["CosmosDb:DatabaseId"];
            var containerId = configuration["CosmosDb:ContainerId"];

            var clientOptions = new CosmosClientOptions()
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };

            cosmosClient = new CosmosClient(connectionString, clientOptions);
            patientContainer = cosmosClient.GetContainer(databaseId, containerId);
        }
        public async Task<Patient> LoadAsync(PatientId patientId)
        {
            if (patientId == null)
            {
                throw new ArgumentNullException(nameof(patientId));
            }

            var aggregateId = $"Patient-{patientId.Value}";
            var sqlQueryText = $"SELECT * FROM c WHERE c.aggregateId = '{aggregateId}'";
            var queryDefinition = new QueryDefinition(sqlQueryText);
            var queryResultSetIterator = patientContainer.GetItemQueryIterator<CosmosEventData>(queryDefinition);
            var allEvents = new List<CosmosEventData>();

            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (CosmosEventData item in currentResultSet)
                {
                    allEvents.Add(item);
                }
            }

            var domainEvents = allEvents.Select(e =>
            {
                var assemblyQualifiedName = JsonConvert.DeserializeObject<string>(e.AssemblyQualifiedName);
                var eventType = Type.GetType(assemblyQualifiedName);
                var data = JsonConvert.DeserializeObject(e.Data, eventType);
                return data as IDomainEvent;
            });

            var aggregate = new Patient(); //Id should be set after loading the events
            aggregate.Load(domainEvents);

            return aggregate;
        }

        public async Task SaveAsync(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient));
            }

            var changes = patient.GetChanges()
              .Select(e => new CosmosEventData()
              {
                  Id = Guid.NewGuid(),
                  AggregateId = $"Patient-{patient.Id}",
                  EventName = e.GetType().Name,
                  Data = JsonConvert.SerializeObject(e),
                  AssemblyQualifiedName = JsonConvert.SerializeObject(e.GetType().AssemblyQualifiedName)
              }).AsEnumerable();

            if (!changes.Any())
            {
                return;
            }

            foreach (var item in changes)
            {
                await patientContainer.CreateItemAsync(item);
            }

            patient.ClearChanges();
        }
    }
}