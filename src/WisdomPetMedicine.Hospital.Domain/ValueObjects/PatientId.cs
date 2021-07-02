using System;

namespace WisdomPetMedicine.Hospital.Domain.ValueObjects
{
    public record PatientId
    {
        public Guid Value { get; init; }

        internal PatientId(Guid value)
        {
            Value = value;
        }

        public static PatientId Create(Guid value)
        {
            return new PatientId(value);
        }
        
        public static implicit operator Guid (PatientId value)
        {
            return value.Value;
        }
    }
}