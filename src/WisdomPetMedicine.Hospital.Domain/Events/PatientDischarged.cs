using System;
using WisdomPetMedicine.Common;

namespace WisdomPetMedicine.Hospital.Domain.Events
{
    public class PatientDischarged : IDomainEvent
    {
        public Guid Id { get; set; }
    }
}