using System;

namespace WisdomPetMedicine.Rescue.Domain.ValueObjects
{
    public record AdopterId
    {
        public Guid Value { get; init; }
        internal AdopterId(Guid value)
        {
            Value = value;
        }

        public static implicit operator Guid (AdopterId id)
        {
            return id.Value;
        }

        public static AdopterId Create(Guid value)
        {
            return new AdopterId(value);
        }
    }
}