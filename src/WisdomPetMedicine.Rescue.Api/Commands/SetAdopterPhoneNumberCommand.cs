using System;

namespace WisdomPetMedicine.Rescue.Api.Commands
{
    public class SetAdopterPhoneNumberCommand
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
    }
}