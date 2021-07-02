using System;

namespace WisdomPetMedicine.Pet.Api.Commands
{
    public class SetNameCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}