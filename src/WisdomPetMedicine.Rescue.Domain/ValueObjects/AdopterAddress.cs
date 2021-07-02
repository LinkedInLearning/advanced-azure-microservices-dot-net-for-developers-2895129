namespace WisdomPetMedicine.Rescue.Domain.ValueObjects
{
    public record AdopterAddress
    {
        public string Street { get; init; }
        public string Number { get; init; }
        public string City { get; init; }
        public string PostalCode { get; init; }
        public string Country { get; init; }

        internal AdopterAddress(string street, string number, string city, string postalCode, string country)
        {
            Street = street;
            Number = number;
            City = city;
            PostalCode = postalCode;
            Country = country;
        }

        public static AdopterAddress Create(string street, string number, string city, string postalCode, string country)
        {
            return new AdopterAddress(street, number, city, postalCode, country);
        }
    }
}