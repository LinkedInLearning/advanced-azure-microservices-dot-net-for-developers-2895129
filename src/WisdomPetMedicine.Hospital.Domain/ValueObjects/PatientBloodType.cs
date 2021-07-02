namespace WisdomPetMedicine.Hospital.Domain.ValueObjects
{
    public record PatientBloodType
    {
        public string Value { get; init; }

        internal PatientBloodType(string value)
        {
            Value = value;
        }

        public static implicit operator string (PatientBloodType bloodType)
        {
            return bloodType.Value;
        }

        public static PatientBloodType Create(string value)
        {
            return new PatientBloodType(value);
        }
    }
}