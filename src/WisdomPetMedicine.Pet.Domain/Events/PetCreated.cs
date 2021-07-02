using System;
using WisdomPetMedicine.Common;

namespace WisdomPetMedicine.Pet.Domain.Events
{
    public class PetCreated : IDomainEvent
    {
        public Guid Id { get; set; }
    }
}