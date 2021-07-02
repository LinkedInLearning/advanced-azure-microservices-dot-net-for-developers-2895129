using System;

namespace WisdomPetMedicine.Rescue.Domain.ValueObjects
{
    public record RescuedAnimalId
    {
        public Guid Value { get; init; }
        internal RescuedAnimalId(Guid value)
        {
            Value = value;
        }

        public static RescuedAnimalId Create(Guid value)
        {
            Validate(value);
            return new RescuedAnimalId(value);
        }

        public static implicit operator Guid(RescuedAnimalId petId)
        {
            return petId.Value;
        }

        private static void Validate(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Id should not be empty", nameof(value));
            }
        }
    }
}