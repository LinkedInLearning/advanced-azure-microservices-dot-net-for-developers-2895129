using WisdomPetMedicine.Common;

namespace WisdomPetMedicine.Rescue.Domain.Events
{
    public static class DomainEvents
    {
        public static readonly DomainEvent<AdoptionRequestCreated> AdoptionRequestCreated = new();
    }
}