using System;

namespace WisdomPetMedicine.Pet.Api.Commands
{
    public class SetDateOfBirthCommand
    {
        public Guid Id { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}