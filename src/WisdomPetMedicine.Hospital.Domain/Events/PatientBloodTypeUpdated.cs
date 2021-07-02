using System;
using WisdomPetMedicine.Common;

namespace WisdomPetMedicine.Hospital.Domain.Events
{
    public class PatientBloodTypeUpdated : IDomainEvent
    {
        public Guid Id { get; set; }
        public string BloodType { get; set; }
    }
}