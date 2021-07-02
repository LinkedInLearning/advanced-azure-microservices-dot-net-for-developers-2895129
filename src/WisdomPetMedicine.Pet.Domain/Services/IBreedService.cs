using WisdomPetMedicine.Pet.Domain.ValueObjects;

namespace WisdomPetMedicine.Pet.Domain.Services
{
    public interface IBreedService
    {
        PetBreed Find(string name);
    }
}