using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenTelemetry.Context.Propagation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using WisdomPetMedicine.Common;
using WisdomPetMedicine.Pet.Api.Commands;
using WisdomPetMedicine.Pet.Api.IntegrationEvents;
using WisdomPetMedicine.Pet.Domain.Events;
using WisdomPetMedicine.Pet.Domain.Repositories;
using WisdomPetMedicine.Pet.Domain.Services;
using WisdomPetMedicine.Pet.Domain.ValueObjects;

namespace WisdomPetMedicine.Pet.Api.ApplicationServices
{
    public class PetApplicationService
    {
        private readonly IPetRepository petRepository;
        private readonly IBreedService breedService;
        private readonly ILogger<PetApplicationService> logger;

        public PetApplicationService(IPetRepository petRepository,
                                     IBreedService breedService,
                                     IConfiguration configuration,
                                     ILogger<PetApplicationService> logger)
        {
            this.petRepository = petRepository;
            this.breedService = breedService;
            this.logger = logger;
            DomainEvents.PetFlaggedForAdoption.Register(async c =>
            {
                var integrationEvent = new PetFlaggedForAdoptionIntegrationEvent()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Breed = c.Breed,
                    Sex = c.Sex,
                    Color = c.Color,
                    DateOfBirth = c.DateOfBirth,
                    Species = c.Species
                };

                await PublishIntegrationEventAsync(integrationEvent,
                                                   configuration["ServiceBus:ConnectionString"],
                                                   configuration["ServiceBus:Adoption:TopicName"]);

            });

            DomainEvents.PetTransferredToHospital.Register(async c =>
            {
                var integrationEvent = new PetTransferredToHospitalIntegrationEvent()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Breed = c.Breed,
                    Sex = c.Sex,
                    Color = c.Color,
                    DateOfBirth = c.DateOfBirth,
                    Species = c.Species
                };
                await PublishIntegrationEventAsync(integrationEvent,
                                                   configuration["ServiceBus:ConnectionString"],
                                                   configuration["ServiceBus:Transfer:TopicName"]);
            });
        }

        public async Task HandleCommandAsync(CreatePetCommand command)
        {
            var pet = new Domain.Entities.Pet(PetId.Create(command.Id));
            pet.SetName(PetName.Create(command.Name));
            pet.SetBreed(PetBreed.Create(command.Breed, breedService));
            pet.SetSex(SexOfPet.Create((SexesOfPets)command.Sex));
            pet.SetColor(PetColor.Create(command.Color));
            pet.SetDateOfBirth(PetDateOfBirth.Create(command.DateOfBirth));
            pet.SetSpecies(PetSpecies.Get(command.Species));
            await petRepository.AddAsync(pet);
        }

        public async Task HandleCommandAsync(SetNameCommand command)
        {
            var pet = await petRepository.GetAsync(PetId.Create(command.Id));
            pet.SetName(PetName.Create(command.Name));
            await petRepository.UpdateAsync(pet);
        }

        public async Task HandleCommandAsync(SetBreedCommand command)
        {
            var pet = await petRepository.GetAsync(PetId.Create(command.Id));
            pet.SetBreed(PetBreed.Create(command.Breed, new FakeBreedService()));
            await petRepository.UpdateAsync(pet);
        }

        public async Task HandleCommandAsync(SetColorCommand command)
        {
            var pet = await petRepository.GetAsync(PetId.Create(command.Id));
            pet.SetColor(PetColor.Create(command.Color));
            await petRepository.UpdateAsync(pet);
        }

        public async Task HandleCommandAsync(SetDateOfBirthCommand command)
        {
            var pet = await petRepository.GetAsync(PetId.Create(command.Id));
            pet.SetDateOfBirth(PetDateOfBirth.Create(command.DateOfBirth));
            await petRepository.UpdateAsync(pet);
        }

        public async Task HandleCommandAsync(FlagForAdoptionCommand command)
        {
            var pet = await petRepository.GetAsync(PetId.Create(command.Id));
            pet.FlagForAdoption();            
        }

        public async Task HandleCommandAsync(TransferToHospitalCommand command)
        {
            var pet = await petRepository.GetAsync(PetId.Create(command.Id));
            pet.TransferToHospital();
        }

        private async Task PublishIntegrationEventAsync(IIntegrationEvent integrationEvent, string connectionString, string topicName)
        {
            var jsonMessage = JsonConvert.SerializeObject(integrationEvent);
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            var client = new ServiceBusClient(connectionString);
            var sender = client.CreateSender(topicName);
            var message = new ServiceBusMessage()
            {
                Body = new BinaryData(body),
                MessageId = Guid.NewGuid().ToString(),
                ContentType = MediaTypeNames.Application.Json,
                Subject = integrationEvent.GetType().FullName
            };

            var serializedMessage = JsonConvert.SerializeObject(message);
            PropagateTracing(message.ApplicationProperties, serializedMessage);

            logger?.LogInformation("Sending message: {payload}", serializedMessage);
            await sender.SendMessageAsync(message);
        }

        private void PropagateTracing(IDictionary<string, object> carrier, string message)
        {
            var propagator = Propagators.DefaultTextMapPropagator;
            using var activitySource = new ActivitySource("pet-api");
            using var activity = activitySource.StartActivity("pet-publisher", ActivityKind.Producer);
            var propagationContext = new PropagationContext(activity.Context, OpenTelemetry.Baggage.Current);
            propagator.Inject(propagationContext, carrier, 
                    (properties, key, value) =>
                    {
                        properties[key] = value;
                    });
            
            activity.SetTag("message", message);
        }
    }
}