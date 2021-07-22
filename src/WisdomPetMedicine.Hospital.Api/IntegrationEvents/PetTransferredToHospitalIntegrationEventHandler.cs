using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WisdomPetMedicine.Hospital.Api.Infrastructure;
using WisdomPetMedicine.Hospital.Domain.Entities;
using WisdomPetMedicine.Hospital.Domain.Repositories;
using WisdomPetMedicine.Hospital.Domain.ValueObjects;

namespace WisdomPetMedicine.Hospital.Api.IntegrationEvents
{
    public class PetTransferredToHospitalIntegrationEventHandler : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IPatientAggregateStore patientAggregateStore;
        private readonly ILogger<PetTransferredToHospitalIntegrationEventHandler> logger;
        private readonly ServiceBusClient client;
        private readonly ServiceBusProcessor processor;
        public PetTransferredToHospitalIntegrationEventHandler(IConfiguration configuration,
                                                               IServiceScopeFactory serviceScopeFactory,
                                                               IPatientAggregateStore patientAggregateStore,
                                                               ILogger<PetTransferredToHospitalIntegrationEventHandler> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.patientAggregateStore = patientAggregateStore;
            this.logger = logger;

            client = new ServiceBusClient(configuration["ServiceBus:ConnectionString"]);
            processor = client.CreateProcessor(configuration["ServiceBus:TopicName"], configuration["ServiceBus:SubscriptionName"]);
            processor.ProcessMessageAsync += Processor_ProcessMessageAsync;
            processor.ProcessErrorAsync += Processor_ProcessErrorAsync;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await processor.StartProcessingAsync(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await processor.StopProcessingAsync(cancellationToken);
        }

        private async Task Processor_ProcessMessageAsync(ProcessMessageEventArgs args)
        {
            var body = args.Message.Body.ToString();
            var theEvent = JsonConvert.DeserializeObject<PetTransferredToHospitalIntegrationEvent>(body);
            await args.CompleteMessageAsync(args.Message);

            logger?.LogInformation($"Received message: {body}");

            PropagateTracing(args.Message.ApplicationProperties, body);

            using var scope = serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<HospitalDbContext>();

            var existingPatient = await dbContext.PatientsMetadata.FindAsync(theEvent.Id);
            if (existingPatient == null)
            {
                dbContext.PatientsMetadata.Add(theEvent);
                await dbContext.SaveChangesAsync();
            }

            var patientId = PatientId.Create(theEvent.Id);
            var patient = new Patient(patientId);
            await patientAggregateStore.SaveAsync(patient);
        }

        private Task Processor_ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            logger.LogError(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private void PropagateTracing(IReadOnlyDictionary<string, object> carrier, string body)
        {
            var propagator = Propagators.DefaultTextMapPropagator;
            using var activitySource = new ActivitySource("hospital-api");
            var parentContext = propagator.Extract(default, carrier, (props, key) =>
            {
                var traceProperties = new List<string>();
                if (props.TryGetValue(key, out var value))
                {
                    traceProperties.Add(value.ToString());
                }
                return traceProperties;
            });
            Baggage.Current = parentContext.Baggage;
            using var activity = activitySource.StartActivity("hospital-consumer", ActivityKind.Consumer, parentContext.ActivityContext);
            activity.SetTag("message", body);
        }
    }
}