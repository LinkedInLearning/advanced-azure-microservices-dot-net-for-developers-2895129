using System;
using WisdomPetMedicine.Common;

namespace WisdomPetMedicine.Hospital.Domain.Events
{
    public class PatientAdmitted : IDomainEvent
    {
        public Guid Id { get; set; }
    }
}