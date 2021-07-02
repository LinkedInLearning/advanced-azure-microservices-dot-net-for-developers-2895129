using System;
using WisdomPetMedicine.Rescue.Domain.Entities;
using WisdomPetMedicine.Rescue.Domain.ValueObjects;
using Xunit;

namespace WisdomPetMedicine.Rescue.Domain.Tests
{
    public class RescuedAnimalTests
    {
        [Fact]
        public void StatusShouldBePendingAfterRequestingAdoption()
        {
            var rescuedAnimal = new RescuedAnimal(RescuedAnimalId.Create(Guid.NewGuid()));
            rescuedAnimal.RequestToAdopt(AdopterId.Create(Guid.NewGuid()));
            Assert.Equal(RescuedAnimalAdoptionStatus.PendingReview, rescuedAnimal.AdoptionStatus);
        }
    }
}