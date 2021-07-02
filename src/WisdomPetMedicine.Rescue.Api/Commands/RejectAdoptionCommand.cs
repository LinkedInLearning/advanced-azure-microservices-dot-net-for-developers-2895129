using System;

namespace WisdomPetMedicine.Rescue.Api.Commands
{
    public class RejectAdoptionCommand
    {
        public Guid PetId { get; set; }
        public Guid AdopterId { get; set; }
    }
}