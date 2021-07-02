using System;
using WisdomPetMedicine.Common;

namespace WisdomPetMedicine.Hospital.Domain.Events
{
    public class PatientCreated : IDomainEvent
    {
        public Guid Id { get; set; }
    }
}