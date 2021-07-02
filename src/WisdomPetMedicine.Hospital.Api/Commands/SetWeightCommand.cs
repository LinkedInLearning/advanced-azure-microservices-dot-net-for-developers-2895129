using System;

namespace WisdomPetMedicine.Hospital.Api.Commands
{
    public class SetWeightCommand
    {
        public Guid Id { get; set; }
        public decimal Weight { get; set; }
    }
}