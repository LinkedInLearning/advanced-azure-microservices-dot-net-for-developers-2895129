using System;

namespace WisdomPetMedicine.Hospital.Api.Commands
{
    public class SetBloodTypeCommand
    {
        public Guid Id { get; set; }
        public string BloodType { get; set; }
    }
}