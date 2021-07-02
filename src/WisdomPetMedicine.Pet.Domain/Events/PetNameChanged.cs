using System;
using WisdomPetMedicine.Common;

namespace WisdomPetMedicine.Pet.Domain.Events
{
    public class PetNameChanged : IDomainEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}