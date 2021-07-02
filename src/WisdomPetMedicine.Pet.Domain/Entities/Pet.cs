using System;
using WisdomPetMedicine.Pet.Domain.Events;
using WisdomPetMedicine.Pet.Domain.Exceptions;
using WisdomPetMedicine.Pet.Domain.ValueObjects;

namespace WisdomPetMedicine.Pet.Domain.Entities
{
    public class Pet
    {
        public Guid Id { get; init; }
        public PetName Name { get; private set; }
        public PetBreed Breed { get; private set; }
        public SexOfPet SexOfPet { get; private set; }
        public PetColor Color { get; private set; }
        public PetDateOfBirth DateOfBirth { get; private set; }
        public PetSpecies Species { get; private set; }

        public Pet(PetId id)
        {
            Id = id;
        }
        public Pet()
        {

        }
        public void SetName(PetName name)
        {
            Name = name;
        }

        public void SetBreed(PetBreed breed)
        {
            Breed = breed;
        }

        public void SetSex(SexOfPet sex)
        {
            SexOfPet = sex;
        }

        public void SetColor(PetColor color)
        {
            Color = color;
        }

        public void SetDateOfBirth(PetDateOfBirth dateOfBirth)
        {
            DateOfBirth = dateOfBirth;
        }

        public void SetSpecies(PetSpecies species)
        {
            Species = species;
        }

        public void FlagForAdoption()
        {
            ValidateStateForAdoption();
            DomainEvents.PetFlaggedForAdoption.Publish(new PetFlaggedForAdoption()
            {
                Id = Id,
                Name = Name,
                Breed = Breed,
                Sex = SexOfPet,
                Color = Color,
                DateOfBirth = DateOfBirth,
                Species = Species
            });
        }

        public void TransferToHospital()
        {
            ValidateStateForTransfer();
            DomainEvents.PetTransferredToHospital.Publish(new PetTransferredToHospital()
            {
                Id = Id,
                Name = Name,
                Breed = Breed,
                Sex = SexOfPet,
                Color = Color,
                DateOfBirth = DateOfBirth,
                Species = Species
            });
        }

        private void ValidateStateForAdoption()
        {
            if (Name == null)
            {
                throw new InvalidPetStateException("Name is missing");
            }
            if (Breed == null)
            {
                throw new InvalidPetStateException("Breed is missing");
            }
            if (SexOfPet == null)
            {
                throw new InvalidPetStateException("Sex is missing");
            }
            if (Color == null)
            {
                throw new InvalidPetStateException("Color is missing");
            }
            if (DateOfBirth == null)
            {
                throw new InvalidPetStateException("Date of birth is missing");
            }
            if (Species == null)
            {
                throw new InvalidPetStateException("Species is missing");
            }
        }

        private void ValidateStateForTransfer()
        {
            if (Name == null)
            {
                throw new InvalidPetStateException("Name is missing");
            }
            if (Breed == null)
            {
                throw new InvalidPetStateException("Breed is missing");
            }
            if (SexOfPet == null)
            {
                throw new InvalidPetStateException("Sex is missing");
            }
            if (Color == null)
            {
                throw new InvalidPetStateException("Color is missing");
            }
            if (DateOfBirth == null)
            {
                throw new InvalidPetStateException("Date of birth is missing");
            }
            if (Species == null)
            {
                throw new InvalidPetStateException("Species is missing");
            }
        }
    }
}