using System;

namespace WisdomPetMedicine.Rescue.Domain.ValueObjects
{
    public record AdopterName
    {
        public string Value { get; init; }

        internal AdopterName(string value)
        {
            Value = value;
        }

        public static AdopterName Create(string value)
        {
            Validate(value);
            return new AdopterName(value);
        }

        private static void Validate(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Name must not be null");
            }

            if (value.Length > 50)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Name must not be longer than 50 characters");
            }
        }
    }
}