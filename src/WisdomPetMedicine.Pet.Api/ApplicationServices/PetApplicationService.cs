using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WisdomPetMedicine.Pet.Api.Commands;
using WisdomPetMedicine.Pet.Domain.Repositories;
using WisdomPetMedicine.Pet.Domain.Services;
using WisdomPetMedicine.Pet.Domain.ValueObjects;

namespace WisdomPetMedicine.Pet.Api.ApplicationServices
{
    public class PetApplicationService
    {
        private readonly IPetRepository petRepository;
        private readonly IBreedService breedService;

        public PetApplicationService(IPetRepository petRepository,
                                     IBreedService breedService)
        {
            this.petRepository = petRepository;
            this.breedService = breedService;
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
    }
}