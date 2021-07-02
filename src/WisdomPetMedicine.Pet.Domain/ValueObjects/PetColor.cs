using System;

namespace WisdomPetMedicine.Pet.Domain.ValueObjects
{
    public record PetColor
    {
        public string Value { get; init; }

        internal PetColor(string value)
        {
            Value = value;
        }

        public static PetColor Create(string value)
        {
            Validate(value);
            return new PetColor(value);
        }

        public static implicit operator string(PetColor color)
        {
            return color.Value;
        }

        private static void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Color cannot be empty or null", nameof(value));
            }
        }
    }
}