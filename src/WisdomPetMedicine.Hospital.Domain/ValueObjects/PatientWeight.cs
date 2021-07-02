using WisdomPetMedicine.Hospital.Domain.Exceptions;

namespace WisdomPetMedicine.Hospital.Domain.ValueObjects
{
    public record PatientWeight
    {
        public decimal Value { get; set; }
        internal PatientWeight(decimal value)
        {
            Value = value;
        }

        public static PatientWeight Create(decimal value)
        {
            Validate(value);
            return new PatientWeight(value);
        }

        public static implicit operator PatientWeight (decimal value)
        {
            return new PatientWeight(value);
        }

        private static void Validate(decimal value)
        {
            if (value < 0)
            {
                throw new InvalidPatientStateException("The weight is invalid");
            }
        }
    }
}