using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using WisdomPetMedicine.Rescue.Api.Infrastructure;
using WisdomPetMedicine.Rescue.Domain.Entities;
using WisdomPetMedicine.Rescue.Domain.Repositories;
using WisdomPetMedicine.Rescue.Domain.ValueObjects;

namespace WisdomPetMedicine.Rescue.Api.IntegrationEvents
{
    public class PetFlaggedForAdoptionIntegrationEventHandler : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<PetFlaggedForAdoptionIntegrationEventHandler> logger;
        private readonly ServiceBusClient client;
        private readonly ServiceBusProcessor processor;
        public PetFlaggedForAdoptionIntegrationEventHandler(IConfiguration configuration,
                                                            IServiceScopeFactory serviceScopeFactory,
                                                            ILogger<PetFlaggedForAdoptionIntegrationEventHandler> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
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
            var theEvent = JsonConvert.DeserializeObject<PetFlaggedForAdoptionIntegrationEvent>(body);
            await args.CompleteMessageAsync(args.Message);

            using var scope = serviceScopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IRescueRepository>();
            var dbContext = scope.ServiceProvider.GetRequiredService<RescueDbContext>();
            dbContext.RescuedAnimalsMetadata.Add(theEvent);
            var rescuedAnimal = new RescuedAnimal(RescuedAnimalId.Create(theEvent.Id));
            await repo.AddRescuedAnimalAsync(rescuedAnimal);
        }

        private Task Processor_ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            logger.LogError(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}