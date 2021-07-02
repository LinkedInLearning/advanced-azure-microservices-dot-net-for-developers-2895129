using System;
using WisdomPetMedicine.Rescue.Domain.ValueObjects;

namespace WisdomPetMedicine.Rescue.Domain.Entities
{
    public class RescuedAnimal
    {
        public Guid Id { get; init; }
        public AdopterId AdopterId { get; private set; }
        public RescuedAnimalAdoptionStatus AdoptionStatus { get; private set; }
        public RescuedAnimal(RescuedAnimalId id)
        {
            Id = id;
        }

        protected RescuedAnimal()
        {

        }

        public void RequestToAdopt(AdopterId adopterId)
        {
            AdopterId = adopterId;
            AdoptionStatus = RescuedAnimalAdoptionStatus.PendingReview;
        }
    }
}