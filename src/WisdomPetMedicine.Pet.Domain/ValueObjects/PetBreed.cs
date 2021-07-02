using System;
using WisdomPetMedicine.Pet.Domain.Services;

namespace WisdomPetMedicine.Pet.Domain.ValueObjects
{
    public record PetBreed
    {
        public string Value { get; init; }

        internal PetBreed(string value)
        {
            Value = value;
        }

        public static PetBreed Create(string value, IBreedService breedService)
        {
            Validate(value, breedService);
            return new PetBreed(value);
        }

        public static implicit operator string(PetBreed breed)
        {
            return breed.Value;
        }

        private static void Validate(string value, IBreedService breedService)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Breed cannot be empty or null", nameof(value));
            }

            var breed = breedService?.Find(value);
            if (breed == null)
            {
                throw new ArgumentException("Breed specified is not valid");
            }
        }
    }
}