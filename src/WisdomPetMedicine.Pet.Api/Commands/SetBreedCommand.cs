using System;

namespace WisdomPetMedicine.Pet.Api.Commands
{
    public class SetBreedCommand
    {
        public Guid Id { get; set; }
        public string Breed { get; set; }
    }
}