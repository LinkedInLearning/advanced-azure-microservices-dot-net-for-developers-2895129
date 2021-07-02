using System;

namespace WisdomPetMedicine.Rescue.Api.Commands
{
    public class RequestAdoptionCommand
    {
        public Guid PetId { get; set; }
        public Guid AdopterId { get; set; }
    }
}