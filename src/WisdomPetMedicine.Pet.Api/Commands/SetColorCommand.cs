using System;

namespace WisdomPetMedicine.Pet.Api.Commands
{
    public class SetColorCommand
    {
        public Guid Id { get; set; }
        public string Color { get; set; }
    }
}