using System;

namespace WisdomPetMedicine.Rescue.Api.Commands
{
    public class CreateAdopterCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Questionnaire Questionnaire { get; set; }
        public Address Address { get; set; }
    }

    public class Questionnaire
    {
        public bool IsActivePerson { get; set; }
        public bool DoYouRent { get; set; }
        public bool HasFencedYard { get; set; }
        public bool HasChildren { get; set; }
    }
    public class Address
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}