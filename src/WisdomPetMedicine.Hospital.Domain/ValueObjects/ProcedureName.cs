namespace WisdomPetMedicine.Hospital.Domain.ValueObjects
{
    public record ProcedureName
    {
        public string Value { get; init; }
        internal ProcedureName(string value)
        {
            Value = value;
        }

        public static ProcedureName Create(string value)
        {
            return new ProcedureName(value);
        }
    }
}