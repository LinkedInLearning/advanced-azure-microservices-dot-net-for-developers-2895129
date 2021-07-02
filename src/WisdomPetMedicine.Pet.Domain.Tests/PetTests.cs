using System;
using WisdomPetMedicine.Pet.Domain.Services;
using WisdomPetMedicine.Pet.Domain.ValueObjects;
using Xunit;

namespace WisdomPetMedicine.Pet.Domain.Tests
{
    public class PetTests
    {
        [Fact]
        public void PetIdCanBeSetToAValidId()
        {
            var newId = Guid.NewGuid();
            var pet = new Entities.Pet(PetId.Create(newId));
            Assert.Equal(newId, pet.Id);
        }

        [Fact]
        public void PetIdCannotBeSetToAnEmptyId()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new Entities.Pet(PetId.Create(Guid.Empty));
            });
        }

        [Fact]
        public void PetNameCanBeSetToAValidName()
        {
            var pet = new Entities.Pet(PetId.Create(Guid.NewGuid()));
            pet.SetName(PetName.Create("Snoopy"));
            Assert.Equal("Snoopy", pet.Name);
        }

        [Fact]
        public void PetNameCannotBeSetToAnInvalidName()
        {
            var pet = new Entities.Pet(PetId.Create(Guid.NewGuid()));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                pet.SetName(PetName.Create(Guid.NewGuid().ToString()));
            });
        }

        [Fact]
        public void PetBreedCanBeSetToAValidBreed()
        {
            var fakeBreedService = new FakeBreedService();
            var pet = new Entities.Pet(PetId.Create(Guid.NewGuid()));
            pet.SetBreed(PetBreed.Create("Beagle", fakeBreedService));
            Assert.Equal("Beagle", pet.Breed);
        }

        [Fact]
        public void PetBreedCannotBeSetToAnInvalidBreed()
        {
            var fakeBreedService = new FakeBreedService();
            var pet = new Entities.Pet(PetId.Create(Guid.NewGuid()));
            Assert.Throws<ArgumentException>(() =>
            {
                pet.SetBreed(PetBreed.Create("Doberman", fakeBreedService));
            });
        }
    }
}