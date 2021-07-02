using System;
using WisdomPetMedicine.Common;

namespace WisdomPetMedicine.Pet.Domain.Events
{
    public class PetColorChanged : IDomainEvent
    {
        public Guid Id { get; set; }

        public string Color { get; set; }
    }
}