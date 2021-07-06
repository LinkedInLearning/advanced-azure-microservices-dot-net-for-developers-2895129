using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WisdomPetMedicine.Rescue.Api.Commands;
using WisdomPetMedicine.Rescue.Domain.Entities;
using WisdomPetMedicine.Rescue.Domain.Events;
using WisdomPetMedicine.Rescue.Domain.Repositories;
using WisdomPetMedicine.Rescue.Domain.ValueObjects;

namespace WisdomPetMedicine.Rescue.Api.ApplicationServices
{
    public class AdopterApplicationService
    {
        private readonly IRescueRepository rescueRepository;

        public AdopterApplicationService(IRescueRepository rescuedAnimalRepository,
                                         IServiceScopeFactory serviceScopeFactory)
        {
            this.rescueRepository = rescuedAnimalRepository;

            DomainEvents.AdoptionRequestCreated.Register(async e =>
            {
                using var scope = serviceScopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IRescueRepository>();
                var rescuedAnimal = await repo.GetRescuedAnimalAsync(RescuedAnimalId.Create(e.RescuedAnimalId));
                rescuedAnimal.RequestToAdopt(AdopterId.Create(e.AdopterId));
                await repo.UpdateRescuedAnimalAsync(rescuedAnimal);
            });
        }

        public async Task HandleCommandAsync(CreateAdopterCommand command)
        {
            var adopter = new Adopter(AdopterId.Create(command.Id));
            adopter.SetName(AdopterName.Create(command.Name));
            adopter.SetAddress(AdopterAddress.Create(command.Address.Street,
                                                     command.Address.Number,
                                                     command.Address.City,
                                                     command.Address.PostalCode,
                                                     command.Address.Country));
            adopter.SetQuestionnaire(AdopterQuestionnaire.Create(command.Questionnaire.IsActivePerson,
                                                                 command.Questionnaire.DoYouRent,
                                                                 command.Questionnaire.HasFencedYard,
                                                                 command.Questionnaire.HasChildren));
            await rescueRepository.AddAdopterAsync(adopter);
        }

        public async Task HandleCommandAsync(RequestAdoptionCommand command)
        {
            var adopter = await rescueRepository.GetAdopterAsync(AdopterId.Create(command.AdopterId));
            adopter.RequestToAdopt(RescuedAnimalId.Create(command.PetId));
            await rescueRepository.UpdateAdopterAsync(adopter);
        }

        public async Task HandleCommandAsync(ApproveAdoptionCommand command)
        {
            var adopter = await rescueRepository.GetAdopterAsync(AdopterId.Create(command.AdopterId));
            adopter.RequestToAdopt(RescuedAnimalId.Create(command.PetId));
            await rescueRepository.UpdateAdopterAsync(adopter);
        }

        public async Task HandleCommandAsync(RejectAdoptionCommand command)
        {
            var adopter = await rescueRepository.GetAdopterAsync(AdopterId.Create(command.AdopterId));
            adopter.RequestToAdopt(RescuedAnimalId.Create(command.PetId));
            await rescueRepository.UpdateAdopterAsync(adopter);
        }
    }
}