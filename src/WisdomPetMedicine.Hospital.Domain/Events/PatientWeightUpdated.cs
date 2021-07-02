using System;
using WisdomPetMedicine.Common;

namespace WisdomPetMedicine.Hospital.Domain.Events
{
    public class PatientWeightUpdated : IDomainEvent
    {
        public Guid Id { get; set; }
        public decimal Value { get; set; }
    }
}